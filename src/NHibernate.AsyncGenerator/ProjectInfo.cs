using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Shared.Extensions;
using NHibernate.AsyncGenerator.Extensions;

namespace NHibernate.AsyncGenerator
{
	public class ProjectInfo : Dictionary<string, DocumentInfo>
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(ProjectInfo));
		
		private readonly SolutionConfiguration _solutionConfiguration;
		private readonly string _asyncProjectFolder;
		private HashSet<string> _ignoredProjectNames;
		private readonly Dictionary<string, Dictionary<string, bool>> _typeNamespaceUniquness = new Dictionary<string, Dictionary<string, bool>>();

		public ProjectInfo(Project project, ProjectConfiguration configuration)
		{
			Configuration = configuration;
			_solutionConfiguration = Configuration.SolutionConfiguration;
			Project = project;
			_asyncProjectFolder = Path.Combine(Path.GetDirectoryName(Project.FilePath), Configuration.AsyncFolder);
		}

		private HashSet<ReferenceLocation> IgnoredReferenceLocation { get; } = new HashSet<ReferenceLocation>();

		private HashSet<IMethodSymbol> AnalyzedSymbols { get; } = new HashSet<IMethodSymbol>();

		internal SolutionInfo SolutionInfo { get; set; }

		internal readonly ProjectConfiguration Configuration;

		public Project Project { get; }

		public bool IsNameUniqueInsideNamespace(INamespaceSymbol namespaceSymbol, string name)
		{
			var result = true;
			while (namespaceSymbol != null && result)
			{
				var namespaceName = namespaceSymbol.ToString();
				if (!_typeNamespaceUniquness.ContainsKey(namespaceName))
				{
					_typeNamespaceUniquness.Add(namespaceName, new Dictionary<string, bool>());
				}
				if (!_typeNamespaceUniquness[namespaceName].ContainsKey(name))
				{
					_typeNamespaceUniquness[namespaceName].Add(name, !namespaceSymbol.ConstituentNamespaces.Any(o => o.GetMembers(name).Any()));
				}
				result &= _typeNamespaceUniquness[namespaceName][name];
				namespaceSymbol = namespaceSymbol.ContainingNamespace;
			}
			return result;
		}

		public async Task Analyze()
		{
			Clear();
			IgnoredReferenceLocation.Clear();
			AnalyzedSymbols.Clear();

			// cache project names to ignore to increase performance
			_ignoredProjectNames = new HashSet<string>(Configuration.IgnoreReferencesFromProjects);
			var ignoreProjects = SolutionInfo?.Configuration?.IgnoreProjectNames;
			if (ignoreProjects != null)
			{
				foreach (var projectName in ignoreProjects)
				{
					_ignoredProjectNames.Add(projectName);
				}
			}

			foreach (var document in Project.Documents.Where(o => !o.FilePath.StartsWith(_asyncProjectFolder)))
			{
				await AnalyzeDocument(document).ConfigureAwait(false);
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

		private async Task<DocumentInfo> AnalyzeDocument(Document document)
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
				if (Configuration.ScanForMissingAsyncMembers)
				{
					await ScanForMissingAsyncMembers(docInfo, declaredSymbol).ConfigureAwait(false);
				}
				foreach (var memberSymbol in declaredSymbol
					.GetMembers()
					.OfType<IMethodSymbol>()
					.Where(Configuration.CanScanMethodFunc))
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

		private async Task ScanForMissingAsyncMembers(DocumentInfo docInfo, INamedTypeSymbol typeSymbol)
		{
			var members = typeSymbol.GetMembers().OfType<IMethodSymbol>().ToLookup(o => o.Name);
			var methodInfos = new List<MethodInfo>();
			foreach (var asyncMember in typeSymbol.AllInterfaces
												  .SelectMany(o => o.GetMembers().OfType<IMethodSymbol>().Where(m => m.Name.EndsWith("Async"))))
			{
				if (typeSymbol.FindImplementationForInterfaceMember(asyncMember) != null)
				{
					continue;
				}
				var nonAsyncName = asyncMember.Name.Remove(asyncMember.Name.LastIndexOf("Async", StringComparison.InvariantCulture));
				var nonAsyncMember = members[nonAsyncName].First(o => o.HaveSameParameters(asyncMember));
				methodInfos.Add(docInfo.GetOrCreateMethodInfo(nonAsyncMember));
			}

			// find all abstract non implemented async methods. Descend base types until we find a non abstract one.
			var baseType = typeSymbol.BaseType;
			while (baseType != null)
			{
				if (!baseType.IsAbstract)
				{
					break;
				}
				foreach (var asyncMember in baseType.GetMembers()
					.OfType<IMethodSymbol>()
					.Where(o => o.IsAbstract && o.Name.EndsWith("Async")))
				{
					var nonAsyncName = asyncMember.Name.Remove(asyncMember.Name.LastIndexOf("Async", StringComparison.InvariantCulture));
					var nonAsyncMember = members[nonAsyncName].FirstOrDefault(o => o.HaveSameParameters(asyncMember));
					if (nonAsyncMember == null)
					{
						Logger.Warn($"Abstract sync counterpart of async member {asyncMember} not found");
						continue;
					}
					methodInfos.Add(docInfo.GetOrCreateMethodInfo(nonAsyncMember));
				}
				baseType = baseType.BaseType;
			}
			
			if (Configuration.ScanMethodsBody)
			{
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
			}
		}

		private class SymbolInfo
		{
			public HashSet<IMethodSymbol> BaseMethods { get; set; }

			public IMethodSymbol OverridenMethod { get; set; }

			public HashSet<IMethodSymbol> Overrides { get; set; }

			public IMethodSymbol MethodSymbol { get; set; }

			public bool IsValid { get; set; }
		}

		private Task<DocumentInfo> GetDocumentInfo(SyntaxReference syntax)
		{
			return GetDocumentInfo(Project.Solution.GetDocument(syntax.SyntaxTree));
		}

		private async Task<DocumentInfo> GetDocumentInfo(Document doc)
		{
			if (doc.Project == Project)
			{
				return await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
			}
			if (Configuration.IgnoreExternalReferences || _ignoredProjectNames.Contains(doc.Project.Name))
			{
				return null;
			}
			if (SolutionInfo == null)
			{
				throw new InvalidOperationException(
					$"Cannot process reference in document ${doc} as it is located outise the project. Set IgnoreExternalRefereces to true or use a SolutionInfo to analyze multiple projects");
			}

			var projectInfo = SolutionInfo.ProjectInfos.First(o => o.Project == doc.Project);
			return await projectInfo.GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
		}

		private bool CanProcessSyntaxReference(SyntaxReference syntax)
		{
			return CanProcessDocument(Project.Solution.GetDocument(syntax.SyntaxTree));
		}

		private bool CanProcessDocument(Document doc)
		{
			if (doc.Project == Project)
			{
				return true;
			}
			if (Configuration.IgnoreExternalReferences || _ignoredProjectNames.Contains(doc.Project.Name))
			{
				return false;
			}
			return SolutionInfo != null;
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
			if (methodSymbol.Name.EndsWith("Async"))
			{
				Logger.Debug($"Symbol {symbol} is already async");
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

			var baseMethods = new HashSet<IMethodSymbol>();
			// check if explicitly implements external interfaces
			if (methodSymbol.MethodKind == MethodKind.ExplicitInterfaceImplementation)
			{
				foreach (var interfaceMember in methodSymbol.ExplicitInterfaceImplementations)
				{
					var syntax = interfaceMember.DeclaringSyntaxReferences.SingleOrDefault();
					if (syntax == null)
					{
						Logger.Warn(
							$"Method {methodSymbol} implements an external interface {interfaceMember} and cannot be made async");
						return new SymbolInfo();
					}
					if (!CanProcessSyntaxReference(syntax))
					{
						continue;
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
				var syntax = interfaceMember.DeclaringSyntaxReferences.SingleOrDefault();
				if (syntax == null)
				{
					Logger.Warn(
						$"Method {methodSymbol} implements an external interface {interfaceMember} and cannot be made async");
					return new SymbolInfo();
				}
				if (!CanProcessSyntaxReference(syntax))
				{
					continue;
				}
				baseMethods.Add(interfaceMember.OriginalDefinition);
			}

			// check if the method is overriding an external method
			var overridenMethod = methodSymbol.OverriddenMethod;
			var overrrides = new HashSet<IMethodSymbol>();
			while (overridenMethod != null)
			{
				var syntax = overridenMethod.DeclaringSyntaxReferences.SingleOrDefault();
				if (syntax == null)
				{
					Logger.Warn($"Method {methodSymbol} overrides an external method {overridenMethod} and cannot be made async");
					return new SymbolInfo();
				}
				if (CanProcessSyntaxReference(syntax))
				{
					overrrides.Add(overridenMethod.OriginalDefinition);
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

			if (_solutionConfiguration.IsSymbolValidFunc != null && !_solutionConfiguration.IsSymbolValidFunc(this, methodSymbol))
			{
				Logger.Warn($"Method {methodSymbol} will be ignored because of user defined function");
				return new SymbolInfo();
			}

			if (methodSymbol.ContainingType.TypeKind == TypeKind.Interface)
			{
				baseMethods.Add(methodSymbol);
			}
			methodSymbol = methodSymbol.OriginalDefinition; // unwrap method

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
			var methodInfos = new List<MethodInfo>();

			// save all overrides
			foreach (var overide in symbolInfo.Overrides)
			{
				syntax = overide.DeclaringSyntaxReferences.Single();
				docInfo = await GetDocumentInfo(syntax).ConfigureAwait(false);
				if (docInfo != null)
				{
					methodInfos.Add(docInfo.GetOrCreateMethodInfo(overide));
				}
			}

			// get and save all interface implementations
			foreach (var interfaceMethod in symbolInfo.BaseMethods)
			{
				syntax = interfaceMethod.DeclaringSyntaxReferences.Single();
				docInfo = await GetDocumentInfo(syntax).ConfigureAwait(false);
				methodInfos.Add(docInfo.GetOrCreateMethodInfo(interfaceMethod));

				var implementations = await SymbolFinder.FindImplementationsAsync(interfaceMethod, Project.Solution).ConfigureAwait(false);
				foreach (var implementation in implementations.OfType<IMethodSymbol>())
				{
					syntax = implementation.DeclaringSyntaxReferences.Single();
					docInfo = await GetDocumentInfo(syntax).ConfigureAwait(false);
					if (docInfo != null)
					{
						methodInfos.Add(docInfo.GetOrCreateMethodInfo(implementation.OriginalDefinition));
					}
				}
			}

			// get and save all derived methods
			if (symbolInfo.OverridenMethod != null)
			{
				var overrides = await SymbolFinder.FindOverridesAsync(symbolInfo.OverridenMethod, Project.Solution).ConfigureAwait(false);
				foreach (var overide in overrides.OfType<IMethodSymbol>())
				{
					syntax = overide.DeclaringSyntaxReferences.Single();
					docInfo = await GetDocumentInfo(syntax).ConfigureAwait(false);
					if (docInfo != null)
					{
						methodInfos.Add(docInfo.GetOrCreateMethodInfo(overide.OriginalDefinition));
					}
				}
				symbolInfo.BaseMethods.Add(symbolInfo.OverridenMethod);
			}

			if (Configuration.ScanMethodsBody)
			{
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
				.Where(o => !o.Location.SourceTree.FilePath.StartsWith(_asyncProjectFolder)))
			{
				if (IgnoredReferenceLocation.Contains(refLocation))
				{
					continue;
				}

				if (refLocation.Document.Project != Project &&
					(
						Configuration.IgnoreExternalReferences ||
						_ignoredProjectNames.Contains(refLocation.Document.Project.Name)
					))
				{
					Logger.Debug($"Ignoring reference ${refLocation} as it is referencing a symbol from another project that was ignored by user");
					IgnoredReferenceLocation.Add(refLocation);
					continue;
				}

				var info = await GetDocumentInfo(refLocation.Document).ConfigureAwait(false);
				if (info == null)
				{
					continue;
				}
				var symbol = info.GetEnclosingMethodOrPropertyOrField(refLocation);
				if (symbol == null)
				{
					Logger.Debug($"Symbol not found for reference ${refLocation}");
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
