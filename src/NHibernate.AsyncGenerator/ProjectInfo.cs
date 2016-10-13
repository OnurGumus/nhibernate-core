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
		private IImmutableSet<string> _ignoredProjectNames;
		private IImmutableSet<Document> _searchDocuments;
		private IImmutableSet<Project> _searchProjects;
		private readonly Dictionary<string, Dictionary<string, bool>> _typeNamespaceUniquness = new Dictionary<string, Dictionary<string, bool>>();

		public ProjectInfo(SolutionInfo solutionInfo, ProjectId projectId, ProjectConfiguration configuration)
		{
			SolutionInfo = solutionInfo;
			Configuration = configuration;
			ProjectId = projectId;
			DirectoryPath = Path.GetDirectoryName(Project.FilePath) + @"\";
		}

		private HashSet<ReferenceLocation> IgnoredReferenceLocation { get; } = new HashSet<ReferenceLocation>();

		private HashSet<ReferenceLocation> ScannedReferenceLocations { get; } = new HashSet<ReferenceLocation>();

		private HashSet<IMethodSymbol> AnalyzedMethodSymbols { get; } = new HashSet<IMethodSymbol>();

		internal SolutionInfo SolutionInfo { get; }

		internal readonly ProjectConfiguration Configuration;

		public string DirectoryPath { get; }

		public ProjectId ProjectId { get; }

		public Project Project => SolutionInfo.Solution.GetProject(ProjectId);

		public Project RemoveGeneratedDocuments()
		{
			var project = Project;
			var asyncProjectFolder = Path.Combine(DirectoryPath, Configuration.AsyncFolder) + @"\";
			// remove all generated documents
			var toRemove = project.Documents.Where(o => o.FilePath.StartsWith(asyncProjectFolder)).Select(doc => doc.Id).ToList();
			foreach (var docId in toRemove)
			{
				project = project.RemoveDocument(docId);
			}
			return project;
		}

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
				_searchDocuments = Configuration.IgnoreExternalReferences
					? GetDocuments().ToImmutableHashSet()
					: SolutionInfo
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
				_searchProjects = Configuration.IgnoreExternalReferences
					? new[] {Project}.ToImmutableHashSet()
					: SolutionInfo
						.ProjectInfos
						.Where(o => !_ignoredProjectNames.Contains(o.Project.Name))
						.Select(o => o.Project)
						.ToImmutableHashSet();
			}
			return _searchProjects;
		}

		public IEnumerable<Document> GetDocuments()
		{
			return Project.Documents;
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

		public void PostAnalyze()
		{
			var methodInfos = new HashSet<MethodInfo>();
			foreach (var pair in this.Where(o => o.Value.Any()))
			{
				foreach (var namespaceInfo in pair.Value.Values)
				{
					foreach (var rootTypeInfo in namespaceInfo.Values.Where(o => o.TypeTransformation != TypeTransformation.None))
					{
						foreach (var typeInfo in rootTypeInfo.GetDescendantTypeInfosAndSelf()
															 .Where(o => o.TypeTransformation != TypeTransformation.None))
						{
							foreach (var methodInfo in typeInfo.MethodInfos.Values)
							{
								methodInfos.Add(methodInfo);
								methodInfo.Analyze();
							}
						}
					}
				}
			}
			foreach (var methodInfo in methodInfos)
			{
				methodInfo.PostAnalyze();
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
					await ScanForTypeReferences(typeInfo, declaredSymbol).ConfigureAwait(false);
				}
				
				if (Configuration.ScanForMissingAsyncMembers)
				{
					await ScanForTypeMissingAsyncMethods(docInfo, declaredSymbol).ConfigureAwait(false);
				}

				foreach (var memberSymbol in declaredSymbol
					.GetMembers()
					.OfType<IMethodSymbol>()
					.Where(o => !AnalyzedMethodSymbols.Contains(o.OriginalDefinition)))
				{
					var conversion = Configuration.MethodConversionFunc(memberSymbol);
					if (conversion == MethodAsyncConversion.None)
					{
						continue;
					}
					var symbolInfo = await AnalyzeMethodSymbol(memberSymbol, true, conversion).ConfigureAwait(false);
					if (!symbolInfo.IsValid)
					{
						// For new types we need to register all method infos in order to copy only methods that are not ignored
						if (symbolInfo == MethodSymbolInfo.Ignore && typeInfo.TypeTransformation == TypeTransformation.NewType)
						{
							docInfo.GetOrCreateMethodInfo(memberSymbol)?.SetIgnore(true);
						}

						continue;
					}
					if (conversion == MethodAsyncConversion.Smart && !await SmartAnalyzeMethod(docInfo, typeInfo, memberSymbol, symbolInfo).ConfigureAwait(false))
					{
						continue;
					}
					await ProcessMethodSymbolInfo(symbolInfo, docInfo).ConfigureAwait(false);
				}
			}
			return docInfo;
		}

		private async Task<bool> SmartAnalyzeMethod(DocumentInfo docInfo, TypeInfo typeInfo, IMethodSymbol memberSymbol, MethodSymbolInfo symbolInfo)
		{
			var methodInfo = docInfo.GetMethodInfo(memberSymbol);
			var created = false;
			if (methodInfo == null)
			{
				methodInfo = docInfo.GetOrCreateMethodInfo(memberSymbol);
				created = true;
			}
			if (methodInfo.Missing || (await methodInfo.FindAsyncCounterpartMethodsWhitinBody(MethodAsyncConterparts).ConfigureAwait(false)).Any())
			{
				return true;
			}
			// Check if the method is required (ie implements an interface) but only if the result will be a new type
			// because we do not want uneeded methods to be copied in a new type
			//if (typeInfo.TypeTransformation == TypeTransformation.NewType && (symbolInfo.InterfaceMethods.Any() || symbolInfo.OverriddenMethods.Any()))
			//{
			//	methodInfo.Required = true;
			//	return false;
			//}
			if (created)
			{
				methodInfo.TypeInfo.MethodInfos.Remove(methodInfo.Node);
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
		/// <param name="typeInfo"></param>
		/// <param name="symbol"></param>
		/// <returns></returns>
		private async Task ScanForTypeReferences(TypeInfo typeInfo, INamedTypeSymbol symbol)
		{
			if (_scannedTypeReferenceSymbols.Contains(symbol.OriginalDefinition))
			{
				return;
			}
			_scannedTypeReferenceSymbols.Add(symbol.OriginalDefinition);

			// references for ctor of the type and the type itself wont have any locations
			var references = await SymbolFinder.FindReferencesAsync(symbol, Project.Solution, GetSearchableDocuments()).ConfigureAwait(false);
			foreach (var refLocation in references.SelectMany(o => o.Locations)
												  .Where(o => !_scannedTypeReferenceLocations.Contains(o)))
			{
				_scannedTypeReferenceLocations.Add(refLocation);
				var info = await GetOrCreateDocumentInfo(refLocation.Document).ConfigureAwait(false);
				if (info == null)
				{
					continue;
				}
				typeInfo.Dependencies.Add(refLocation);

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
					var methodInfo = info.GetOrCreateMethodInfo(methodNode, true);
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
						var refTypeInfo = info.GetOrCreateTypeInfo(type);
						if (refTypeInfo.TypeReferences.Contains(refLocation))
						{
							continue;
						}
						refTypeInfo.TypeReferences.Add(refLocation);
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
			var members = typeSymbol.GetMembers()
									.OfType<IMethodSymbol>()
									.ToLookup(o =>
										o.MethodKind == MethodKind.ExplicitInterfaceImplementation
											? o.Name.Split('.').Last()
											: o.Name);
			var methodInfos = new List<MethodInfo>();

			foreach (var asyncMember in typeSymbol.AllInterfaces
												  .SelectMany(o => o.GetMembers().OfType<IMethodSymbol>()
												  .Where(m => m.Name.EndsWith("Async"))))
			{
				var bug = asyncMember.Parameters; // An NullReferenceException exception will be thrown if this line will be removed. 

				// Skip if there is already an implementation defined
				var impl = typeSymbol.FindImplementationForInterfaceMember(asyncMember);
				if (impl != null)
				{
					continue;
					/*
					if (impl.ContainingAssembly.Name != typeSymbol.ContainingAssembly.Name)
					{
						continue;
					}*/
				}
				var nonAsyncName = asyncMember.Name.Remove(asyncMember.Name.LastIndexOf("Async", StringComparison.InvariantCulture));
				if (!members.Contains(nonAsyncName))
				{
					continue;
				}
				var nonAsyncMember = members[nonAsyncName].First(o => o.HaveSameParameters(asyncMember));
				var methodInfo = docInfo.GetOrCreateMethodInfo(nonAsyncMember);
				methodInfo.Missing = true;
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
					methodInfos.Add(methodInfo);
				}
				baseType = baseType.BaseType;
			}
			
			if (Configuration.ScanMethodsBody)
			{
				var asnycCounterparts = new List<AsyncCounterpartMethod>();
				foreach (var methodInfo in methodInfos)
				{
					asnycCounterparts.AddRange(await methodInfo.FindAsyncCounterpartMethodsWhitinBody(MethodAsyncConterparts).ConfigureAwait(false));
				}
				foreach (var group in asnycCounterparts.GroupBy(o => o.MethodSymbol))
				{
					await ScanAllMethodReferenceLocations(group.Key).ConfigureAwait(false);
				}

				//foreach (var group in methodInfos
				//	.SelectMany(o => o.FindAsyncCounterpartMethodsWhitinBody(MethodAsyncConterparts))
				//	.GroupBy(o => o.MethodSymbol))
				//{
				//	await ScanAllMethodReferenceLocations(group.Key).ConfigureAwait(false);
				//}
			}
		}

		private class MethodSymbolInfo
		{
			public static readonly MethodSymbolInfo Invalid = new MethodSymbolInfo();

			public static readonly MethodSymbolInfo Ignore = new MethodSymbolInfo();

			public HashSet<IMethodSymbol> InterfaceMethods { get; set; }

			public HashSet<IMethodSymbol> AsyncInterfaceMethods { get; set; }

			public HashSet<IMethodSymbol> OverriddenMethods { get; set; }

			public IMethodSymbol BaseOverriddenMethod { get; set; }

			public IMethodSymbol MethodSymbol { get; set; }

			public MethodAsyncConversion? MethodConversion { get; set; }

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
			if (Configuration.IgnoreExternalReferences)
			{
				return null;
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

		private async Task<MethodSymbolInfo> AnalyzeMethodSymbol(IMethodSymbol methodSymbol, bool memorize = true, MethodAsyncConversion? conversion = null)
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
				if (conversion != MethodAsyncConversion.Smart)
				{
					Logger.Debug($"Symbol {methodSymbol} is already async");
				}
				return MethodSymbolInfo.Ignore;
			}
			if (methodSymbol.MethodKind != MethodKind.Ordinary && methodSymbol.MethodKind != MethodKind.ExplicitInterfaceImplementation)
			{
				if (conversion != MethodAsyncConversion.Smart)
				{
					Logger.Warn($"Method {methodSymbol} is a {methodSymbol.MethodKind} and cannot be made async");
				}
				return MethodSymbolInfo.Invalid;
			}

			if (methodSymbol.Parameters.Any(o => o.RefKind == RefKind.Out))
			{
				if (conversion != MethodAsyncConversion.Smart)
				{
					Logger.Warn($"Method {methodSymbol} has out parameters and cannot be made async");
				}
				return MethodSymbolInfo.Invalid;
			}

			if (methodSymbol.DeclaringSyntaxReferences.SingleOrDefault() == null)
			{
				if (conversion != MethodAsyncConversion.Smart)
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
														 .Where(o => o.Name == methodSymbol.Name + "Async" && !o.IsSealed && (o.IsVirtual || o.IsAbstract || o.IsOverride))
														 .SingleOrDefault(o => o.HaveSameParameters(methodSymbol));
					if (asyncConterPart == null)
					{
						if (!asyncMethods.Any() || (asyncMethods.Any() && !overridenMethod.IsOverride && !overridenMethod.IsVirtual))
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
				else if (CanProcessSyntaxReference(syntax))
				{
					overrrides.Add(overridenMethod.OriginalDefinition);
				}
				if (memorize && !AnalyzedMethodSymbols.Contains(overridenMethod.OriginalDefinition))
				{
					AnalyzedMethodSymbols.Add(overridenMethod.OriginalDefinition);
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

			// check if the method is implementing an external interface, if true skip as we cannot modify externals
			// FindImplementationForInterfaceMember will find the first implementation method starting from the deepest base class
			var type = methodSymbol.ContainingType;
			foreach (var interfaceMember in type.AllInterfaces
												.SelectMany(
													o => o.GetMembers(methodSymbol.Name)
														  .Where(
															  m =>
															  {
																  // find out if the method implements the interface member or an override 
																  // method that implements it
																  var impl = type.FindImplementationForInterfaceMember(m);
																  return methodSymbol.Equals(impl) || overrrides.Any(ov => ov.Equals(impl));
															  }
															))
														  .OfType<IMethodSymbol>())
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

			// Verify if there is already an async counterpart for this method
			IMethodSymbol asyncCounterpart;
			if (Configuration.FindAsyncCounterpart != null)
			{
				asyncCounterpart = await Configuration.FindAsyncCounterpart.Invoke(Project, methodSymbol.OriginalDefinition, false).ConfigureAwait(false);
			}
			else
			{
				asyncCounterpart = methodSymbol.GetAsyncCounterpart();
			}
			if (asyncCounterpart != null)
			{
				Logger.Debug($"Method {methodSymbol} has already an async counterpart {asyncCounterpart}");
				_cachedMethodSymbolInfos.Add(methodSymbol.OriginalDefinition, MethodSymbolInfo.Ignore);
				return MethodSymbolInfo.Ignore;
			}

			var isSymbolValidFunc = SolutionInfo.Configuration.IsSymbolValidFunc;
			if (isSymbolValidFunc != null && !isSymbolValidFunc(this, methodSymbol))
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
				BaseOverriddenMethod = overridenMethod?.DeclaringSyntaxReferences.Length > 0
					? overridenMethod.OriginalDefinition
					: null,
				MethodConversion = conversion,
				OverriddenMethods = overrrides
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
			mainMethodInfo.Conversion = methodSymbolInfo.MethodConversion;

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
						interfaceMethodInfo?.ExternalRelatedMethods.Add(implementation);
						continue;
					}
					var methodInfo = docInfo.GetOrCreateMethodInfo(implementation.OriginalDefinition);
					if (interfaceMethodInfo != null)
					{
						methodInfo.RelatedMethods.Add(interfaceMethodInfo);
						interfaceMethodInfo.RelatedMethods.Add(methodInfo);
					}
					else
					{
						methodInfo.ExternalRelatedMethods.Add(interfaceMethod);
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
				if (baseMethodInfo != null && !baseMethod.IsAbstract)
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
						baseMethodInfo?.ExternalRelatedMethods.Add(overriddenMethod);
						continue;
					}
					var methodInfo = docInfo.GetOrCreateMethodInfo(overriddenMethod.OriginalDefinition);
					if (baseMethodInfo != null)
					{
						methodInfo.RelatedMethods.Add(baseMethodInfo);
						baseMethodInfo.RelatedMethods.Add(methodInfo);
					}
					else
					{
						methodInfo.ExternalRelatedMethods.Add(baseMethod);
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
				var asnycCounterparts = new List<AsyncCounterpartMethod>();
				foreach (var methodInfo in bodyScanMethodInfos)
				{
					asnycCounterparts.AddRange(await methodInfo.FindAsyncCounterpartMethodsWhitinBody(MethodAsyncConterparts).ConfigureAwait(false));
				}
				foreach (var group in asnycCounterparts.GroupBy(o => o.MethodSymbol))
				{
					await ScanAllMethodReferenceLocations(group.Key).ConfigureAwait(false);
				}
				//foreach (var group in bodyScanMethodInfos
				//	.SelectMany(o => o.FindAsyncCounterpartMethodsWhitinBody(MethodAsyncConterparts))
				//	.GroupBy(o => o.MethodSymbol))
				//{
				//	await ScanAllMethodReferenceLocations(group.Key).ConfigureAwait(false);
				//}
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
				.Where(o => !ScannedReferenceLocations.Contains(o)))
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
				var symbolInfo = await AnalyzeMethodSymbol(refMethodSymbol, false).ConfigureAwait(false);
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
				methodInfo.References.Add(refLocation);

				// find the real method on that reference as FindReferencesAsync will also find references to base and interface methods
				var nameNode = methodInfo.Node.DescendantNodes()
							   .OfType<SimpleNameSyntax>()
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
				var invokedSymbol = (IMethodSymbol) docInfo.SemanticModel.GetSymbolInfo(nameNode).Symbol;
				var invokedMethodDocInfoTask = invokedSymbol.DeclaringSyntaxReferences
																  .Select(GetOrCreateDocumentInfo)
																  .SingleOrDefault();
				if (invokedMethodDocInfoTask != null)
				{
					var invokedMethodDocInfo = await invokedMethodDocInfoTask;
					if (invokedMethodDocInfo != null)
					{
						var invokedMethodInfo = invokedMethodDocInfo.GetOrCreateMethodInfo(invokedSymbol);
						if (!invokedMethodInfo.InvokedBy.Contains(methodInfo))
						{
							invokedMethodInfo.InvokedBy.Add(methodInfo);
						}
					}
				}
				await ProcessMethodSymbolInfo(symbolInfo, docInfo, depth).ConfigureAwait(false);
			}
		}

		#endregion

	}
}
