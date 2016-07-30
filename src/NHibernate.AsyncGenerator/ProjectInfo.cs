using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using NHibernate.AsyncGenerator.Extensions;

namespace NHibernate.AsyncGenerator
{
	public class ProjectInfo : Dictionary<string, DocumentInfo>
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(ProjectInfo));
		
		private readonly SolutionConfiguration _solutionConfiguration;
		private readonly string _asyncProjectFolder;
		private IImmutableSet<string> _ignoredProjectNames;
		private IImmutableSet<Document> _searchDocuments;
		private IImmutableSet<Project> _searchProjects;
		private readonly Dictionary<string, Dictionary<string, bool>> _typeNamespaceUniquness = new Dictionary<string, Dictionary<string, bool>>();

		public ProjectInfo(Project project, ProjectConfiguration configuration)
		{
			Configuration = configuration;
			_solutionConfiguration = Configuration.SolutionConfiguration;
			Project = project;
			_asyncProjectFolder = Path.Combine(Path.GetDirectoryName(Project.FilePath), Configuration.AsyncFolder);
		}

		private HashSet<ReferenceLocation> IgnoredReferenceLocation { get; } = new HashSet<ReferenceLocation>();

		private HashSet<ReferenceLocation> ScannedReferenceLocations { get; } = new HashSet<ReferenceLocation>();

		private HashSet<IMethodSymbol> AnalyzedMethodSymbols { get; } = new HashSet<IMethodSymbol>();

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

		private void SetIgnoredProjectNames()
		{
			if (_ignoredProjectNames != null)
			{
				return ;
			}
			// cache project names to ignore to increase performance
			var ignoredProjectNames = new HashSet<string>(Configuration.IgnoreReferencesFromProjects);
			var ignoreProjects = SolutionInfo?.Configuration?.IgnoreProjectNames;
			if (ignoreProjects != null)
			{
				foreach (var projectName in ignoreProjects)
				{
					ignoredProjectNames.Add(projectName);
				}
			}
			_ignoredProjectNames = ignoredProjectNames.ToImmutableHashSet();
		}

		/// <summary>
		/// Get all documents that can be used when searching with SymbolFinder
		/// </summary>
		/// <returns></returns>
		private IImmutableSet<Document> GetSearchableDocuments()
		{
			if (_searchDocuments == null)
			{
				_searchDocuments = SolutionInfo
					.ProjectInfos
					.Where(o => !_ignoredProjectNames.Contains(o.Project.Name))
					.SelectMany(o => o.GetDocuments())
					.ToImmutableHashSet();
			}
			return _searchDocuments;
		}

		/// <summary>
		/// Get all projects that can be used for searching with SymbolFinder
		/// </summary>
		/// <returns></returns>
		private IImmutableSet<Project> GetSearchableProjects()
		{
			if (_searchProjects == null)
			{
				_searchProjects = SolutionInfo
					.ProjectInfos
					.Where(o => !_ignoredProjectNames.Contains(o.Project.Name))
					.Select(o => o.Project)
					.ToImmutableHashSet();
			}
			return _searchProjects;
		}

		public IEnumerable<Document> GetDocuments()
		{
			return Project.Documents.Where(o => !o.FilePath.StartsWith(_asyncProjectFolder));
		}

		public async Task Analyze()
		{
			Clear();
			IgnoredReferenceLocation.Clear();
			AnalyzedMethodSymbols.Clear();
			_ignoredProjectNames = null;
			SetIgnoredProjectNames();
			foreach (var document in GetDocuments())
			{
				await AnalyzeDocument(document).ConfigureAwait(false);
			}
		}

		public async Task PostAnalyze()
		{
			foreach (var document in GetDocuments())
			{
				await AnalyzeDocument(document).ConfigureAwait(false);
			}
		}


		private async Task<DocumentInfo> AnalyzeDocument(Document document)
		{
			if (document.Project != Project)
			{
				throw new NotSupportedException("Multiple Project for ProjectInfo");
			}
			if (!Configuration.CanScanDocumentFunc(document))
			{
				return null;
			}
			var docInfo = await CreateDocumentInfo(document).ConfigureAwait(false);
			foreach (var typeDeclaration in docInfo.RootNode
												.DescendantNodes()
												.OfType<TypeDeclarationSyntax>())
			{
				var declaredSymbol = docInfo.SemanticModel.GetDeclaredSymbol(typeDeclaration);
				var typeInfo = docInfo.GetOrCreateTypeInfo(typeDeclaration);
				var typeTransform = Configuration.TypeTransformationFunc?.Invoke(declaredSymbol) ?? TypeTransformation.None;
				typeInfo.TypeTransformation = typeTransform;
				if (typeTransform == TypeTransformation.None)
				{
					continue;
				}

				// if the type have to be defined as a new type then we need to find all references to that type 
				if (typeTransform == TypeTransformation.NewType)
				{
					await ScanForTypeReferences(declaredSymbol).ConfigureAwait(false);
				}
				
				if (Configuration.ScanForMissingAsyncMembers)
				{
					await ScanForTypeMissingAsyncMethods(docInfo, declaredSymbol).ConfigureAwait(false);
				}

				foreach (var memberSymbol in declaredSymbol
					.GetMembers()
					.Where(o => o.Locations.All(l => !l.SourceTree.FilePath.StartsWith(_asyncProjectFolder)))
					.OfType<IMethodSymbol>()
					.Where(o => !AnalyzedMethodSymbols.Contains(o.OriginalDefinition)))
				{
					var conversion = Configuration.MethodConversionFunc(memberSymbol);
					if (conversion == MethodAsyncConversion.None)
					{
						continue;
					}
					var symbolInfo = AnalyzeMethodSymbol(memberSymbol, true, conversion == MethodAsyncConversion.Smart);
					if (!symbolInfo.IsValid)
					{
						continue;
					}

					if (conversion == MethodAsyncConversion.Smart && !SmartAnalyzeMethod(docInfo, memberSymbol, symbolInfo))
					{
						continue;
					}
					await ProcessMethodSymbolInfo(symbolInfo, docInfo).ConfigureAwait(false);
				}
			}
			return docInfo;
		}

		private bool SmartAnalyzeMethod(DocumentInfo docInfo, IMethodSymbol memberSymbol, MethodSymbolInfo symbolInfo)
		{
			var methodInfo = docInfo.GetOrCreateMethodInfo(memberSymbol);
			if (methodInfo.Missing)
			{
				return true;
			}

			if (methodInfo.FindAsyncCounterpartMethodsWhitinBody(MethodAsyncConterparts).Any())
			{
				return true;
			}
			return false;
		}

		private bool IsAnyParentOrSelfTypeTransformedAsNewType(INamedTypeSymbol typeSymbol)
		{
			if (Configuration.TypeTransformationFunc == null)
			{
				return false;
			}
			while (typeSymbol != null)
			{
				if (Configuration.TypeTransformationFunc(typeSymbol) == TypeTransformation.NewType)
				{
					return true;
				}
				typeSymbol = typeSymbol.ContainingType;
			}
			return false;
		}

		#region ScanForTypeReferences

		private readonly HashSet<ISymbol> _scannedTypeReferenceSymbols = new HashSet<ISymbol>();
		private readonly HashSet<ReferenceLocation> _scannedTypeReferenceLocations = new HashSet<ReferenceLocation>();

		/// <summary>
		/// When a type needs to be defined as a new type we need to find all references to them.
		/// Reference can point to a variable, field, base type, argument definition
		/// </summary>
		/// <param name="symbol"></param>
		/// <returns></returns>
		private async Task ScanForTypeReferences(INamedTypeSymbol symbol)
		{
			if (_scannedTypeReferenceSymbols.Contains(symbol.OriginalDefinition))
			{
				return;
			}
			_scannedTypeReferenceSymbols.Add(symbol.OriginalDefinition);

			// references for ctor of the type and the type itself wont have any locations
			var references = await SymbolFinder.FindReferencesAsync(symbol, Project.Solution, GetSearchableDocuments()).ConfigureAwait(false);
			foreach (var refLocation in references.SelectMany(o => o.Locations)
												  .Where(o => !_scannedTypeReferenceLocations.Contains(o))
												  .Where(o => !o.Location.SourceTree.FilePath.StartsWith(_asyncProjectFolder)))
			{
				_scannedTypeReferenceLocations.Add(refLocation);
				var info = await GetOrCreateDocumentInfo(refLocation.Document).ConfigureAwait(false);
				if (info == null)
				{
					continue;
				}
				// we need to find the type where the reference location is
				var node = info.RootNode.DescendantNodes(descendIntoTrivia:true)
					.First(
						o =>
						{
							if (o.IsKind(SyntaxKind.GenericName))
							{
								return o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken)).Span ==
									   refLocation.Location.SourceSpan;
							}
							return o.Span == refLocation.Location.SourceSpan;
						});

				var methodNode = node.Ancestors().OfType<MethodDeclarationSyntax>().FirstOrDefault();
				if (methodNode != null)
				{
					var methodInfo = info.GetOrCreateMethodInfo(methodNode);
					//methodInfo.Ignore = false;
					if (methodInfo.TypeReferences.Contains(refLocation))
					{
						continue;
					}
					methodInfo.TypeReferences.Add(refLocation);
				}
				else
				{
					var type = node.Ancestors().OfType<TypeDeclarationSyntax>().FirstOrDefault();
					if (type != null) 
					{
						var typeInfo = info.GetOrCreateTypeInfo(type);
						if (typeInfo.TypeReferences.Contains(refLocation))
						{
							continue;
						}
						typeInfo.TypeReferences.Add(refLocation);
					}
					else // can happen when declaring a Name in a using statement
					{
						var namespaceNode = node.Ancestors().OfType<NamespaceDeclarationSyntax>().FirstOrDefault();
						var namespaceInfo = info.GetNamespaceInfo(namespaceNode, true);
						if (namespaceInfo.TypeReferences.Contains(refLocation))
						{
							continue;
						}
						namespaceInfo.TypeReferences.Add(refLocation);
					}
				}
			}
		}

		#endregion

		internal readonly Dictionary<IMethodSymbol, IMethodSymbol> MethodAsyncConterparts = new Dictionary<IMethodSymbol, IMethodSymbol>();

		private async Task ScanForTypeMissingAsyncMethods(DocumentInfo docInfo, INamedTypeSymbol typeSymbol)
		{
			var members = typeSymbol.GetMembers().OfType<IMethodSymbol>().ToLookup(o => o.Name);
			var methodInfos = new List<MethodInfo>();
			foreach (var asyncMember in typeSymbol.AllInterfaces
												  .SelectMany(o => o.GetMembers().OfType<IMethodSymbol>().Where(m => m.Name.EndsWith("Async"))))
			{
				// FindImplementationForInterfaceMember will find also the implementation of an already generated type so we need an extra check 
				var impl = typeSymbol.FindImplementationForInterfaceMember(asyncMember);
				if (impl != null)
				{
					var location = impl.Locations.SingleOrDefault();
					if (location == null || !location.SourceTree.FilePath.StartsWith(_asyncProjectFolder))
					{
						continue;
					}
				}
				var nonAsyncName = asyncMember.Name.Remove(asyncMember.Name.LastIndexOf("Async", StringComparison.InvariantCulture));
				if (!members.Contains(nonAsyncName))
				{
					continue;
				}
				var nonAsyncMember = members[nonAsyncName].First(o => o.HaveSameParameters(asyncMember));
				var methodInfo = docInfo.GetOrCreateMethodInfo(nonAsyncMember);
				methodInfo.Missing = true;
				//methodInfo.Ignore = false;
				methodInfos.Add(methodInfo);
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
					var methodInfo = docInfo.GetOrCreateMethodInfo(nonAsyncMember);
					methodInfo.Missing = true;
					//methodInfo.Ignore = false;
					methodInfos.Add(methodInfo);
				}
				baseType = baseType.BaseType;
			}
			
			if (Configuration.ScanMethodsBody)
			{
				foreach (var group in methodInfos
					.SelectMany(o => o.FindAsyncCounterpartMethodsWhitinBody(MethodAsyncConterparts))
					.GroupBy(o => o.MethodSymbol))
				{
					await ScanAllMethodReferenceLocations(group.Key).ConfigureAwait(false);
				}
			}
		}

		private class MethodSymbolInfo
		{
			public static readonly MethodSymbolInfo Invalid = new MethodSymbolInfo();

			public HashSet<IMethodSymbol> InterfaceMethods { get; set; }

			public HashSet<IMethodSymbol> AsyncInterfaceMethods { get; set; }

			public HashSet<IMethodSymbol> OverriddenMethods { get; set; }

			public IMethodSymbol BaseOverriddenMethod { get; set; }

			public IMethodSymbol MethodSymbol { get; set; }

			public bool IsValid { get; set; }
		}

		internal Task<DocumentInfo> GetOrCreateDocumentInfo(SyntaxReference syntax)
		{
			return GetOrCreateDocumentInfo(Project.Solution.GetDocument(syntax.SyntaxTree));
		}

		internal async Task<DocumentInfo> GetOrCreateDocumentInfo(Document doc)
		{
			if (!Configuration.CanScanDocumentFunc(doc))
			{
				return null;
			}

			if (doc.Project == Project)
			{
				return await CreateDocumentInfo(doc).ConfigureAwait(false);
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
			return await projectInfo.CreateDocumentInfo(doc).ConfigureAwait(false);
		}

		internal DocumentInfo GetDocumentInfo(SyntaxReference syntax)
		{
			return GetDocumentInfo(Project.Solution.GetDocument(syntax.SyntaxTree));
		}

		internal DocumentInfo GetDocumentInfo(Document document)
		{
			if (document.Project == Project)
			{
				return ContainsKey(document.FilePath) ? this[document.FilePath] : null;
			}
			var projectInfo = SolutionInfo.ProjectInfos.FirstOrDefault(o => o.Project == document.Project);
			return projectInfo?.GetDocumentInfo(document);
		}

		private async Task<DocumentInfo> CreateDocumentInfo(Document document)
		{
			if (ContainsKey(document.FilePath))
			{
				return this[document.FilePath];
			}
			var info = new DocumentInfo(this, document);
			await info.Initialize().ConfigureAwait(false);
			Add(document.FilePath, info);
			return info;
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

		private readonly Dictionary<IMethodSymbol, MethodSymbolInfo> _cachedMethodSymbolInfos = new Dictionary<IMethodSymbol, MethodSymbolInfo>();

		private MethodSymbolInfo AnalyzeMethodSymbol(IMethodSymbol methodSymbol, bool memorize = true, bool skipLog =false)
		{
			if (_cachedMethodSymbolInfos.ContainsKey(methodSymbol.OriginalDefinition))
			{
				return _cachedMethodSymbolInfos[methodSymbol.OriginalDefinition];
			}
			if (memorize && !AnalyzedMethodSymbols.Contains(methodSymbol.OriginalDefinition))
			{
				AnalyzedMethodSymbols.Add(methodSymbol.OriginalDefinition);
			}
			if (methodSymbol.Name.EndsWith("Async"))
			{
				if (!skipLog)
				{
					Logger.Debug($"Symbol {methodSymbol} is already async");
				}
				return MethodSymbolInfo.Invalid;
			}
			if (methodSymbol.MethodKind != MethodKind.Ordinary && methodSymbol.MethodKind != MethodKind.ExplicitInterfaceImplementation)
			{
				if (!skipLog)
				{
					Logger.Warn($"Method {methodSymbol} is a {methodSymbol.MethodKind} and cannot be made async");
				}
				return MethodSymbolInfo.Invalid;
			}

			if (methodSymbol.Parameters.Any(o => o.RefKind == RefKind.Out))
			{
				if (!skipLog)
				{
					Logger.Warn($"Method {methodSymbol} has out parameters and cannot be made async");
				}
				return MethodSymbolInfo.Invalid;
			}

			if (methodSymbol.DeclaringSyntaxReferences.SingleOrDefault() == null)
			{
				if (!skipLog)
				{
					Logger.Warn($"Method {methodSymbol} is external and cannot be made async");
				}
				return MethodSymbolInfo.Invalid;
			}

			var interfaceMethods = new HashSet<IMethodSymbol>();
			var asyncMethods = new HashSet<IMethodSymbol>();
			// check if explicitly implements external interfaces
			if (methodSymbol.MethodKind == MethodKind.ExplicitInterfaceImplementation)
			{
				foreach (var interfaceMember in methodSymbol.ExplicitInterfaceImplementations)
				{
					var syntax = interfaceMember.DeclaringSyntaxReferences.SingleOrDefault();
					if (methodSymbol.ContainingAssembly.Name != interfaceMember.ContainingAssembly.Name)
					{
						// check if the member has an async counterpart that is not implemented in the current type (missing member)
						var asyncConterPart = interfaceMember.ContainingType.GetMembers()
															 .OfType<IMethodSymbol>()
															 .Where(o => o.Name == methodSymbol.Name + "Async")
															 .SingleOrDefault(o => o.HaveSameParameters(methodSymbol));
						if (asyncConterPart == null)
						{
							Logger.Warn(
								$"Method {methodSymbol} implements an external interface {interfaceMember} and cannot be made async");
							return MethodSymbolInfo.Invalid;
						}
						asyncMethods.Add(asyncConterPart);
					}
					if (memorize && !AnalyzedMethodSymbols.Contains(interfaceMember.OriginalDefinition))
					{
						AnalyzedMethodSymbols.Add(interfaceMember.OriginalDefinition);
					}
					if (!CanProcessSyntaxReference(syntax))
					{
						continue;
					}
					interfaceMethods.Add(interfaceMember.OriginalDefinition);
				}
			}

			// check if the method is implementing an external interface, if true skip as we cannot modify externals
			var type = methodSymbol.ContainingType;
			foreach (var interfaceMember in type.AllInterfaces
												.SelectMany(
													o => o.GetMembers(methodSymbol.Name)
														  .Where(m => type.FindImplementationForInterfaceMember(m) != null)
														  .OfType<IMethodSymbol>()))
			{
				var syntax = interfaceMember.DeclaringSyntaxReferences.SingleOrDefault();
				if (syntax == null || methodSymbol.ContainingAssembly.Name != interfaceMember.ContainingAssembly.Name)
				{
					// check if the member has an async counterpart that is not implemented in the current type (missing member)
					var asyncConterPart = interfaceMember.ContainingType.GetMembers()
														 .OfType<IMethodSymbol>()
														 .Where(o => o.Name == methodSymbol.Name + "Async")
														 .SingleOrDefault(o => o.HaveSameParameters(methodSymbol));
					if (asyncConterPart == null)
					{
						Logger.Warn(
							$"Method {methodSymbol} implements an external interface {interfaceMember} and cannot be made async");
						return MethodSymbolInfo.Invalid;
					}
					asyncMethods.Add(asyncConterPart);
				}
				if (memorize && !AnalyzedMethodSymbols.Contains(interfaceMember.OriginalDefinition))
				{
					AnalyzedMethodSymbols.Add(interfaceMember.OriginalDefinition);
				}
				if (!CanProcessSyntaxReference(syntax))
				{
					continue;
				}
				interfaceMethods.Add(interfaceMember.OriginalDefinition);
			}

			// check if the method is overriding an external method
			var overridenMethod = methodSymbol.OverriddenMethod;
			var overrrides = new HashSet<IMethodSymbol>();
			while (overridenMethod != null)
			{
				var syntax = overridenMethod.DeclaringSyntaxReferences.SingleOrDefault();
				if (methodSymbol.ContainingAssembly.Name != overridenMethod.ContainingAssembly.Name)
				{
					// check if the member has an async counterpart that is not implemented in the current type (missing member)
					var asyncConterPart = overridenMethod.ContainingType.GetMembers()
														 .OfType<IMethodSymbol>()
														 .Where(o => o.Name == methodSymbol.Name + "Async" && !o.IsSealed)
														 .SingleOrDefault(o => o.HaveSameParameters(methodSymbol));
					if (asyncConterPart == null)
					{
						if (!asyncMethods.Any() ||
						(asyncMethods.Any() && !overridenMethod.IsOverride && !overridenMethod.IsVirtual))
						{
							Logger.Warn($"Method {methodSymbol} overrides an external method {overridenMethod} and cannot be made async");
							return MethodSymbolInfo.Invalid;
						}
					}
					else
					{
						asyncMethods.Add(asyncConterPart);
					}
				}
				if (memorize && !AnalyzedMethodSymbols.Contains(overridenMethod.OriginalDefinition))
				{
					AnalyzedMethodSymbols.Add(overridenMethod.OriginalDefinition);
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
				Logger.Debug($"Method {methodSymbol} will be ignored because of user defined function");
				_cachedMethodSymbolInfos.Add(methodSymbol.OriginalDefinition, MethodSymbolInfo.Invalid);
				return MethodSymbolInfo.Invalid;
			}

			if (methodSymbol.ContainingType.TypeKind == TypeKind.Interface)
			{
				interfaceMethods.Add(methodSymbol);
			}
			methodSymbol = methodSymbol.OriginalDefinition; // unwrap method

			var result = new MethodSymbolInfo
			{
				IsValid = true,
				MethodSymbol = methodSymbol,
				AsyncInterfaceMethods = asyncMethods,
				InterfaceMethods = interfaceMethods,
				BaseOverriddenMethod = overridenMethod?.OriginalDefinition,
				OverriddenMethods = overrrides,
			};
			_cachedMethodSymbolInfos.Add(methodSymbol, result);
			return result;
		}

		private readonly HashSet<MethodSymbolInfo> _processedMethodSymbolInfos = new HashSet<MethodSymbolInfo>();
		private readonly HashSet<IMethodSymbol> _processedMethodSymbols = new HashSet<IMethodSymbol>();

		private async Task ProcessMethodSymbolInfo(MethodSymbolInfo methodSymbolInfo, DocumentInfo methodDocInfo, int depth = 0)
		{
			if (_processedMethodSymbolInfos.Contains(methodSymbolInfo))
			{
				return;
			}
			_processedMethodSymbolInfos.Add(methodSymbolInfo);

			var mainMethodInfo = methodDocInfo.GetOrCreateMethodInfo(methodSymbolInfo.MethodSymbol);
			foreach (var asyncMethod in methodSymbolInfo.AsyncInterfaceMethods)
			{
				mainMethodInfo.ExternalAsyncMethods.Add(asyncMethod);
			}

			DocumentInfo docInfo;
			SyntaxReference syntax;
			var bodyScanMethodInfos = new HashSet<MethodInfo>();
			var referenceScanMethods = new HashSet<IMethodSymbol>();

			// get and save all interface implementations
			foreach (var interfaceMethod in methodSymbolInfo.InterfaceMethods)
			{
				if (_processedMethodSymbols.Contains(interfaceMethod))
				{
					continue;
				}
				_processedMethodSymbols.Add(interfaceMethod);

				referenceScanMethods.Add(interfaceMethod);
				// try to get MethodInfo
				syntax = interfaceMethod.DeclaringSyntaxReferences.Single();
				docInfo = await GetOrCreateDocumentInfo(syntax).ConfigureAwait(false);
				var interfaceMethodInfo = docInfo?.GetOrCreateMethodInfo(interfaceMethod.OriginalDefinition);
				

				var implementations = await SymbolFinder.FindImplementationsAsync(interfaceMethod, Project.Solution, GetSearchableProjects()).ConfigureAwait(false);
				foreach (var implementation in implementations.OfType<IMethodSymbol>())
				{
					syntax = implementation.DeclaringSyntaxReferences.Single();
					docInfo = await GetOrCreateDocumentInfo(syntax).ConfigureAwait(false);
					if (docInfo == null)
					{
						interfaceMethodInfo?.ExternalDependencies.Add(implementation);
						continue;
					}
					var methodInfo = docInfo.GetOrCreateMethodInfo(implementation.OriginalDefinition);
					if (interfaceMethodInfo != null)
					{
						methodInfo.Dependencies.Add(interfaceMethodInfo);
						interfaceMethodInfo.Dependencies.Add(methodInfo);
					}
					else
					{
						methodInfo.ExternalDependencies.Add(interfaceMethod);
					}
					
					bodyScanMethodInfos.Add(methodInfo);
				}
			}

			// get and save all derived methods
			var baseMethod = methodSymbolInfo.BaseOverriddenMethod ??
							 (methodSymbolInfo.MethodSymbol.IsVirtual || methodSymbolInfo.MethodSymbol.IsAbstract ? methodSymbolInfo.MethodSymbol : null);
			if (baseMethod != null && !_processedMethodSymbols.Contains(baseMethod))
			{
				_processedMethodSymbols.Add(baseMethod);

				referenceScanMethods.Add(baseMethod);
				// try to get MethodInfo
				syntax = baseMethod.DeclaringSyntaxReferences.Single();
				docInfo = await GetOrCreateDocumentInfo(syntax).ConfigureAwait(false);
				var baseMethodInfo = docInfo?.GetOrCreateMethodInfo(baseMethod);
				if (baseMethodInfo != null && baseMethod.IsAbstract)
				{
					bodyScanMethodInfos.Add(baseMethodInfo);
				}
				
				var overrides = await SymbolFinder.FindOverridesAsync(baseMethod, Project.Solution, GetSearchableProjects()).ConfigureAwait(false);
				foreach (var overriddenMethod in overrides.OfType<IMethodSymbol>())
				{
					syntax = overriddenMethod.DeclaringSyntaxReferences.Single();
					docInfo = await GetOrCreateDocumentInfo(syntax).ConfigureAwait(false);
					if (docInfo == null)
					{
						baseMethodInfo?.ExternalDependencies.Add(overriddenMethod);
						continue;
					}
					var methodInfo = docInfo.GetOrCreateMethodInfo(overriddenMethod.OriginalDefinition);
					if (baseMethodInfo != null)
					{
						methodInfo.Dependencies.Add(baseMethodInfo);
						baseMethodInfo.Dependencies.Add(methodInfo);
					}
					else
					{
						methodInfo.ExternalDependencies.Add(baseMethod);
					}
					
					if (!overriddenMethod.IsAbstract)
					{
						bodyScanMethodInfos.Add(methodInfo);
					}
				}
			}

			if (baseMethod == null && !methodSymbolInfo.InterfaceMethods.Any()) //TODO: what about hiding methods
			{
				referenceScanMethods.Add(methodSymbolInfo.MethodSymbol);
				bodyScanMethodInfos.Add(mainMethodInfo);
			}

			if (Configuration.ScanMethodsBody)
			{
				foreach (var group in bodyScanMethodInfos
					.SelectMany(o => o.FindAsyncCounterpartMethodsWhitinBody(MethodAsyncConterparts))
					.GroupBy(o => o.MethodSymbol))
				{
					await ScanAllMethodReferenceLocations(group.Key).ConfigureAwait(false);
				}
			}

			foreach (var methodToScan in referenceScanMethods)
			{
				await ScanAllMethodReferenceLocations(methodToScan, depth).ConfigureAwait(false);
			}
		}

		#region ScanAllMethodReferenceLocations

		private readonly HashSet<IMethodSymbol> _scannedMethodReferenceSymbols = new HashSet<IMethodSymbol>();

		private async Task ScanAllMethodReferenceLocations(IMethodSymbol methodSymbol, int depth = 0)
		{
			if (_scannedMethodReferenceSymbols.Contains(methodSymbol))
			{
				return;
			}
			_scannedMethodReferenceSymbols.Add(methodSymbol);

			var references =
				await SymbolFinder.FindReferencesAsync(methodSymbol, Project.Solution, GetSearchableDocuments()).ConfigureAwait(false);

			depth++;
			foreach (var refLocation in references.SelectMany(o => o.Locations)
				.Where(o => !IgnoredReferenceLocation.Contains(o))
				.Where(o => !ScannedReferenceLocations.Contains(o))
				.Where(o => !o.Location.SourceTree.FilePath.StartsWith(_asyncProjectFolder)))
			{
				if (IgnoredReferenceLocation.Contains(refLocation))
				{
					continue;
				}
				ScannedReferenceLocations.Add(refLocation);

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

				var docInfo = await GetOrCreateDocumentInfo(refLocation.Document).ConfigureAwait(false);
				if (docInfo == null)
				{
					continue;
				}
				var symbol = docInfo.GetEnclosingMethodOrPropertyOrField(refLocation);
				if (symbol == null)
				{
					Logger.Debug($"Symbol not found for reference ${refLocation}");
					continue;
				}

				var refMethodSymbol = symbol as IMethodSymbol;
				if (refMethodSymbol == null)
				{
					IgnoredReferenceLocation.Add(refLocation);
					continue;
				}
				// dont check if the method was already analyzed as there can be many references inside one method
				var symbolInfo = AnalyzeMethodSymbol(refMethodSymbol, false);
				if (!symbolInfo.IsValid)
				{
					IgnoredReferenceLocation.Add(refLocation);
					continue;
				}
				// check if we already processed the reference
				if (docInfo.ContainsReference(symbolInfo.MethodSymbol, refLocation))
				{
					continue;
				}
				// save the reference as it can be made async
				var methodInfo = docInfo.GetOrCreateMethodInfo(symbolInfo.MethodSymbol);
				/*
				if (methodInfo.Ignore)
				{
					//methodInfo.Ignore = false;
				}*/
				methodInfo.References.Add(refLocation);

				await ProcessMethodSymbolInfo(symbolInfo, docInfo, depth).ConfigureAwait(false);
			}
		}

		#endregion

	}
}
