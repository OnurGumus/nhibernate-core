using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Shared.Extensions;

namespace NHibernate.AsyncGenerator
{
	public class ProjectInfo : Dictionary<string, DocumentInfo>
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(ProjectInfo));

		public ProjectInfo(Project project)
		{
			Project = project;
		}

		public Project Project { get; }

		public async Task Analyze()
		{
			Clear();
			IgnoredReferenceLocation.Clear();
			foreach (var document in Project.Documents.Where(o => !o.FilePath.Contains(@"\Async\")))
			{
				await AddDocument(document).ConfigureAwait(false);
			}
		}

		private async Task<DocumentInfo> GetOrCreateDocumentInfo(Document document)
		{
			if (ContainsKey(document.FilePath))
			{
				return this[document.FilePath];
			}
			var info = new DocumentInfo(this, document)
			{
				RootNode = (CompilationUnitSyntax)await document.GetSyntaxRootAsync().ConfigureAwait(false),
				SemanticModel = await document.GetSemanticModelAsync().ConfigureAwait(false)
			};
			Add(document.FilePath, info);
			return info;
		}

		private async Task<DocumentInfo> AddDocument(Document document)
		{
			if (document.Project != Project)
			{
				throw new NotSupportedException("Multiple Project for ProjectInfo");
			}

			var docInfo = await GetOrCreateDocumentInfo(document).ConfigureAwait(false);

			foreach (var typeDeclaration in docInfo.RootNode
												.DescendantNodes()
												.OfType<TypeDeclarationSyntax>())
			{
				var declaredSymbol = docInfo.SemanticModel.GetDeclaredSymbol(typeDeclaration);
				foreach (var memberSymbol in declaredSymbol
					.GetMembers()
					.Where(o => o.GetAttributes().Any(a => a.AttributeClass.Name == "AsyncAttribute")))
				{
					var symbolInfo = AnalizeSymbol(memberSymbol);
					if (!symbolInfo.IsValid)
					{
						continue;
					}
					await ProcessSymbolInfo(symbolInfo).ConfigureAwait(false);
				}
			}
			return docInfo;
		}

		private HashSet<ReferenceLocation> IgnoredReferenceLocation { get; } = new HashSet<ReferenceLocation>();

		private HashSet<IMethodSymbol> AnalyzedSymbols { get; } = new HashSet<IMethodSymbol>();

		private class SymbolInfo
		{
			public HashSet<IMethodSymbol> BaseMethods { get; set; }

			public IMethodSymbol OverridenMethod { get; set; }

			public HashSet<IMethodSymbol> Overrides { get; set; }

			public IMethodSymbol MethodSymbol { get; set; }

			public bool IsValid { get; set; }
		}

		private SymbolInfo AnalizeSymbol(ISymbol symbol)
		{
			// check if the symbol is a method otherwise skip
			var methodSymbol = symbol as IMethodSymbol;
			if (methodSymbol == null)
			{
				Logger.Warn($"Symbol {symbol} is not a method and cannot be made async");
				return new SymbolInfo();
			}
			if (methodSymbol.MethodKind != MethodKind.Ordinary && methodSymbol.MethodKind != MethodKind.ExplicitInterfaceImplementation)
			{
				Logger.Warn($"Method {methodSymbol} is a {methodSymbol.MethodKind} and cannot be made async");
				return new SymbolInfo();
			}

			if (methodSymbol.Parameters.Any(o => o.RefKind == RefKind.Out))
			{
				Logger.Warn($"Method {methodSymbol} has out parameters and cannot be made async");
				return new SymbolInfo();
			}

			if (methodSymbol.DeclaringSyntaxReferences.SingleOrDefault() == null)
			{
				Logger.Warn($"Method {methodSymbol} is external and cannot be made async");
				return new SymbolInfo();
			}
			if (methodSymbol.ContainingNamespace.ToString().StartsWith("NHibernate.Test") && 
				(
				new []
			{
				"TestFixtureSetUp",
				"TestFixtureTearDown",
				"OnSetUp",
				"OnTearDown",
				"AppliesTo",
				"Configure"
			}.Contains(methodSymbol.Name) ||
				methodSymbol.ContainingType.Name == "NorthwindDbCreator"
			) )
			{
				Logger.Info($"Setup test method {methodSymbol} will be ignored");
				return new SymbolInfo();
			}

			var baseMethods = new HashSet<IMethodSymbol>();
			if (methodSymbol.ContainingType.TypeKind == TypeKind.Interface)
			{
				baseMethods.Add(methodSymbol);
			}
			methodSymbol = methodSymbol.OriginalDefinition; // unwrap method

			// check if explicitly implements external interfaces
			if (methodSymbol.MethodKind == MethodKind.ExplicitInterfaceImplementation)
			{
				foreach (var interfaceMember in methodSymbol.ExplicitInterfaceImplementations)
				{
					var synatx = interfaceMember.DeclaringSyntaxReferences.SingleOrDefault();
					if (synatx == null)
					{
						Logger.Warn(
							$"Method {methodSymbol} implements an external interface {interfaceMember} and cannot be made async");
						return new SymbolInfo();
					}
					baseMethods.Add(interfaceMember.OriginalDefinition);
				}
			}

			// check if the method is implementing an external interface if the skip as we cannot modify externals
			var type = methodSymbol.ContainingType;
			foreach (var interfaceMember in type.AllInterfaces
												.SelectMany(
													o => o.GetMembers(methodSymbol.Name)
														  .Where(m => type.FindImplementationForInterfaceMember(m) != null)
														  .OfType<IMethodSymbol>()))
			{
				var synatx = interfaceMember.DeclaringSyntaxReferences.SingleOrDefault();
				if (synatx == null)
				{
					Logger.Warn(
						$"Method {methodSymbol} implements an external interface {interfaceMember} and cannot be made async");
					return new SymbolInfo();
				}
				baseMethods.Add(interfaceMember.OriginalDefinition);
			}

			// check if the method is overriding an external method
			var overridenMethod = methodSymbol.OverriddenMethod;
			var overrrides = new HashSet<IMethodSymbol>();
			while (overridenMethod != null)
			{
				overrrides.Add(overridenMethod.OriginalDefinition);
				var synatx = overridenMethod.DeclaringSyntaxReferences.SingleOrDefault();
				if (synatx == null)
				{
					Logger.Warn($"Method {methodSymbol} overrides an external method {overridenMethod} and cannot be made async");
					return new SymbolInfo();
				}
				if (overridenMethod.OverriddenMethod != null)
				{
					overridenMethod = overridenMethod.OverriddenMethod;
				}
				else
				{
					break;
				}
			}
			return new SymbolInfo
			{
				IsValid = true,
				MethodSymbol = methodSymbol,
				BaseMethods = baseMethods,
				OverridenMethod = overridenMethod?.OriginalDefinition,
				Overrides = overrrides
			};
		}

		private async Task ProcessSymbolInfo(SymbolInfo symbolInfo, int depth = 0)
		{
			DocumentInfo docInfo;
			SyntaxReference syntax;
			Document doc;
			var methodInfos = new List<MethodInfo>();

			// save all overrides
			foreach (var overide in symbolInfo.Overrides)
			{
				syntax = overide.DeclaringSyntaxReferences.Single();
				doc = Project.Solution.GetDocument(syntax.SyntaxTree);
				docInfo = await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
				methodInfos.Add(docInfo.GetOrCreateMethodInfo(overide));
			}

			// get and save all interface implementations
			foreach (var interfaceMethod in symbolInfo.BaseMethods)
			{
				syntax = interfaceMethod.DeclaringSyntaxReferences.Single();
				doc = Project.Solution.GetDocument(syntax.SyntaxTree);
				docInfo = await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
				methodInfos.Add(docInfo.GetOrCreateMethodInfo(interfaceMethod));

				var implementations = await SymbolFinder.FindImplementationsAsync(interfaceMethod, Project.Solution).ConfigureAwait(false);
				foreach (var implementation in implementations.OfType<IMethodSymbol>())
				{
					syntax = implementation.DeclaringSyntaxReferences.Single();
					doc = Project.Solution.GetDocument(syntax.SyntaxTree);
					docInfo = await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
					methodInfos.Add(docInfo.GetOrCreateMethodInfo(implementation.OriginalDefinition));
				}
			}

			// get and save all derived methods
			if (symbolInfo.OverridenMethod != null)
			{
				var overrides = await SymbolFinder.FindOverridesAsync(symbolInfo.OverridenMethod, Project.Solution).ConfigureAwait(false);
				foreach (var overide in overrides.OfType<IMethodSymbol>())
				{
					syntax = overide.DeclaringSyntaxReferences.Single();
					doc = Project.Solution.GetDocument(syntax.SyntaxTree);
					docInfo = await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
					methodInfos.Add(docInfo.GetOrCreateMethodInfo(overide.OriginalDefinition));
				}
				symbolInfo.BaseMethods.Add(symbolInfo.OverridenMethod);
			}

			foreach (var group in methodInfos
				.SelectMany(o => o.FindAsyncCounterpartMethodsWhitinBody())
				.Where(o => !AnalyzedSymbols.Contains(o.MethodSymbol))
				.GroupBy(o => o.MethodSymbol))
			{
				AnalyzedSymbols.Add(group.Key);
				await GetAllReferenceLocations(
					await SymbolFinder.FindReferencesAsync(group.Key, Project.Solution).ConfigureAwait(false), true)
					.ConfigureAwait(false);
			}

			if (symbolInfo.MethodSymbol.HidesBaseMethodsByName ||
				symbolInfo.MethodSymbol.DeclaredAccessibility != Accessibility.Public ||
				!symbolInfo.BaseMethods.Any())
			{
				symbolInfo.BaseMethods.Add(symbolInfo.MethodSymbol);
			}

			foreach (var baseMethod in symbolInfo.BaseMethods.Where(o => !AnalyzedSymbols.Contains(o)))
			{
				AnalyzedSymbols.Add(baseMethod);
				await GetAllReferenceLocations(
					await SymbolFinder.FindReferencesAsync(baseMethod, Project.Solution).ConfigureAwait(false), false, depth)
					.ConfigureAwait(false);
			}
		}

		private async Task GetAllReferenceLocations(IEnumerable<ReferencedSymbol> references, bool bodyToAsyncMethod = false, int depth = 0)
		{
			depth++;
			foreach (var refLocation in references.SelectMany(o => o.Locations)
				.Where(o => !o.Location.SourceTree.FilePath.Contains(@"\Async\")))
			{
				if (IgnoredReferenceLocation.Contains(refLocation))
				{
					continue;
				}
				var info = await GetOrCreateDocumentInfo(refLocation.Document).ConfigureAwait(false);
				var symbol = info.GetEnclosingMethodOrPropertyOrField(refLocation);
				if (symbol == null)
				{
					continue;
				}

				var symbolInfo = AnalizeSymbol(symbol);
				if (!symbolInfo.IsValid)
				{
					IgnoredReferenceLocation.Add(refLocation);
					continue;
				}

				if (bodyToAsyncMethod)
				{
					// check if we already processed the reference
					if (info.ContainsBodyToAsyncMethodReference(symbolInfo.MethodSymbol, refLocation))
					{
						continue;
					}
					// save the reference as it can be made async
					info.GetOrCreateMethodInfo(symbolInfo.MethodSymbol).BodyToAsyncMethodsReferences.Add(refLocation);
				}
				else
				{
					// check if we already processed the reference
					if (info.ContainsReference(symbolInfo.MethodSymbol, refLocation))
					{
						continue;
					}
					// save the reference as it can be made async
					info.GetOrCreateMethodInfo(symbolInfo.MethodSymbol).References.Add(refLocation);

				}
				await ProcessSymbolInfo(symbolInfo, depth).ConfigureAwait(false);
			}
		}

	}
}
