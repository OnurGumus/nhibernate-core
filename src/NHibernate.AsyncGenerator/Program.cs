using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using NHibernate.AsyncGenerator.Extensions;
using Nito.AsyncEx;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace NHibernate.AsyncGenerator
{
	public class MethodReferenceResult
	{
		public MethodReferenceResult(ReferenceLocation reference, SimpleNameSyntax referenceNode)
		{
			ReferenceLocation = reference;
			ReferenceNode = referenceNode;
		}

		public SimpleNameSyntax ReferenceNode { get; }

		public ReferenceLocation ReferenceLocation { get; }

		public bool CanBeAsync { get; internal set; }

		public bool UsedAsReturnValue { get; internal set; }

		public bool LastStatement { get; internal set; }

		public bool InsideCondition { get; internal set; }

		public bool PassedAsArgument { get; internal set; }

		public bool MustBeAwaited { get; set; }
	}

	public class MethodAnalyzeResult
	{
		public List<MethodReferenceResult> ReferenceResults { get; } = new List<MethodReferenceResult>();

		public bool CanBeCompletelyAsync => CanBeAsnyc && ReferenceResults.All(o => o.CanBeAsync);

		public bool CanBeAsnyc { get; internal set; }

		public bool CanSkipAsync { get; internal set; }

		public bool MustRunSynchronized { get; internal set; }

		public bool HasBody { get; internal set; }

		public bool Yields { get; internal set; }

		public bool IsEmpty { get; internal set; }
	}

	internal class MethodInfo
	{
		public MethodInfo(TypeInfo typeInfo, IMethodSymbol symbol, MethodDeclarationSyntax node)
		{
			TypeInfo = typeInfo;
			Symbol = symbol;
			Node = node;
		}

		public TypeInfo TypeInfo { get; }

		public IMethodSymbol Symbol { get; }

		public MethodDeclarationSyntax Node { get; }

		public HashSet<ReferenceLocation> References { get; } = new HashSet<ReferenceLocation>();

		public MethodReferenceResult AnalyzeReference(ReferenceLocation reference)
		{
			var docInfo = TypeInfo.NamespaceInfo.DocumentInfo;
			var nameNode = Node.DescendantNodes()
							   .OfType<SimpleNameSyntax>()
							   .First(
								   o =>
								   {
									   if (o.IsKind(SyntaxKind.GenericName))
									   {
										   return o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken)).Span ==
												  reference.Location.SourceSpan;
									   }
									   return o.Span == reference.Location.SourceSpan;
								   });
			var result = new MethodReferenceResult(reference, nameNode)
			{
				CanBeAsync = true
			};
			// verify how the method is used
			var statementNode = nameNode.Ancestors().OfType<StatementSyntax>().First();
			if (statementNode.IsKind(SyntaxKind.ReturnStatement))
			{
				result.UsedAsReturnValue = true;
			}
			else if (Symbol.ReturnsVoid && statementNode.IsKind(SyntaxKind.ExpressionStatement))
			{
				// check if the reference is the last statement to execute
				var nextNode =
					statementNode.Ancestors()
									   .OfType<BlockSyntax>()
									   .First()
									   .DescendantNodesAndTokens()
									   .First(o => o.SpanStart >= statementNode.Span.End);
				// check if the reference is the last statement in the method before returning
				if (nextNode.IsKind(SyntaxKind.ReturnStatement) || nextNode.Span.End == Node.Span.End)
				{
					result.LastStatement = true;
				}
			}

			if (statementNode.DescendantNodes().OfType<ConditionalExpressionSyntax>().Any())
			{
				result.InsideCondition = true;
			}

			var invocationNode = nameNode.Ancestors().OfType<InvocationExpressionSyntax>().FirstOrDefault();
			if (invocationNode != null) // method is invocated
			{
				if (invocationNode.Parent.IsKind(SyntaxKind.SelectClause)) // await is not supported in select clause
				{
					result.CanBeAsync = false;
					Console.WriteLine($"Cannot await async method in a select clause:\r\n{invocationNode.Parent}\r\n");
				}
				else
				{
					var anonFunctionNode = nameNode.Ancestors().OfType<AnonymousFunctionExpressionSyntax>().FirstOrDefault();
					if (anonFunctionNode?.AsyncKeyword.IsMissing == false)
					{
						result.CanBeAsync = false;
						Console.WriteLine($"Cannot await async method in an non async anonymous function:\r\n{anonFunctionNode}\r\n");
					}
				}

			}
			else if (nameNode.Parent.IsKind(SyntaxKind.Argument)) // method is passed as an argument check if the types matches
			{
				var methodArgTypeInfo = docInfo.SemanticModel.GetTypeInfo(nameNode);
				if (methodArgTypeInfo.ConvertedType?.TypeKind == TypeKind.Delegate)
				{
					var delegateMethod = (IMethodSymbol) methodArgTypeInfo.ConvertedType.GetMembers("Invoke").First();
					
					if (!delegateMethod.IsAsync)
					{
						result.CanBeAsync = false;
						Console.WriteLine($"Cannot pass an async method as parameter to a non async Delegate method:\r\n{delegateMethod}\r\n");
					}
					else
					{
						result.PassedAsArgument = true;
						var argumentMethodSymbol = (IMethodSymbol)docInfo.SemanticModel.GetSymbolInfo(nameNode).Symbol;
						if (!argumentMethodSymbol.ReturnType.Equals(delegateMethod.ReturnType)) // i.e IList<T> -> IEnumerable<T>
						{
							result.MustBeAwaited = true;
						}
					}
				}
				else
				{
					throw new NotSupportedException("Unknown method usage");
				}
			}
			else
			{
				throw new NotSupportedException("Unknown method usage");
			}
			return result;
		}

		// check if all the references can be converted to async
		public MethodAnalyzeResult Analyze()
		{
			if (Node.Body == null)
			{
				return new MethodAnalyzeResult();
			}
			var result = new MethodAnalyzeResult
			{
				HasBody = true
			};

			foreach (var reference in References.OrderByDescending(o => o.Location.SourceSpan.Start))
			{
				result.ReferenceResults.Add(AnalyzeReference(reference));
			}

			var counter = new MethodStatementsCounterVisitior();
			counter.Visit(Node.Body);

			if (counter.TotalYields > 0)
			{
				result.Yields = true;
			}
			if (counter.TotalStatements == 0)
			{
				result.IsEmpty = true;
			}
			if (result.ReferenceResults.Any(o => o.CanBeAsync))
			{
				result.CanBeAsnyc = true;
			}

			if (Node.AttributeLists
					.SelectMany(o => o.Attributes.Where(a => a.Name.ToString() == "MethodImpl"))
					.Any(o => o.ArgumentList.Arguments.Any(a => a.Expression.ToString() == "MethodImplOptions.Synchronized")))
			{
				result.MustRunSynchronized = true;
			}
			else if (counter.InvocationExpressions.Count == 1)
			{
				var invocation = counter.InvocationExpressions[0];
				if (Symbol.ReturnsVoid)
				{
					// if there is only one await and it is the last statement to be executed we can just forward the task
					if (result.ReferenceResults.All(o => o.LastStatement) &&
						invocation.Parent.IsKind(SyntaxKind.ExpressionStatement) &&
						counter.TotalStatements == 1)
					{
						result.CanSkipAsync = true;
					}
				}
				else
				{
					// if there is only one await and it is used as a reutorn value we can just forward the task
					if (result.ReferenceResults.All(o => o.UsedAsReturnValue) &&
						invocation.IsReturned() &&
						counter.TotalStatements == 1)
					{
						result.CanSkipAsync = true;
					}
				}
			}
			return result;
		}
	}

	public class MethodStatementsCounterVisitior : CSharpSyntaxRewriter
	{
		public List<InvocationExpressionSyntax> InvocationExpressions { get; } = new List<InvocationExpressionSyntax>();

		public int TotalYields { get; private set; }

		public int TotalStatements { get; private set; }

		public override SyntaxNode VisitInvocationExpression(InvocationExpressionSyntax node)
		{
			InvocationExpressions.Add(node);
			return base.VisitInvocationExpression(node);
		}

		public override SyntaxNode Visit(SyntaxNode node)
		{
			if (!node.IsKind(SyntaxKind.Block) && node is StatementSyntax)
			{
				TotalStatements++;
			}
			return base.Visit(node);
		}

		public override SyntaxNode VisitYieldStatement(YieldStatementSyntax node)
		{
			TotalYields++;
			return base.VisitYieldStatement(node);
		}
	}

	internal class TypeInfo
	{
		public TypeInfo(NamespaceInfo namespaceInfo, INamedTypeSymbol symbol, TypeDeclarationSyntax node)
		{
			NamespaceInfo = namespaceInfo;
			Symbol = symbol;
			Node = node;
		}

		public NamespaceInfo NamespaceInfo { get; }

		public INamedTypeSymbol Symbol { get; }

		public TypeDeclarationSyntax Node { get; }

		public Dictionary<IMethodSymbol, MethodInfo> MethodInfos { get; } = new Dictionary<IMethodSymbol, MethodInfo>();

		public Dictionary<INamedTypeSymbol, TypeInfo> TypeInfos { get; } = new Dictionary<INamedTypeSymbol, TypeInfo>();

		public MethodInfo GetMethodInfo(IMethodSymbol symbol, bool create = false)
		{
			if (MethodInfos.ContainsKey(symbol))
			{
				return MethodInfos[symbol];
			}
			if (!create)
			{
				return null;
			}
			var location = symbol.Locations.Single(o => o.SourceTree.FilePath == Node.SyntaxTree.FilePath);
			var memberNode = Node.DescendantNodes()
									 .OfType<MethodDeclarationSyntax>()
									 .Single(o => o.ChildTokens().SingleOrDefault(t => t.IsKind(SyntaxKind.IdentifierToken)).Span == location.SourceSpan);
			var asyncMember = new MethodInfo(this, symbol, memberNode);
			MethodInfos.Add(symbol, asyncMember);
			return asyncMember;
		}

	}

	internal class NamespaceInfo
	{
		public NamespaceInfo(DocumentInfo documentInfo,  INamespaceSymbol symbol, NamespaceDeclarationSyntax node)
		{
			DocumentInfo = documentInfo;
			Symbol = symbol;
			Node = node;
		}

		public DocumentInfo DocumentInfo { get; }

		public INamespaceSymbol Symbol { get; }

		public NamespaceDeclarationSyntax Node { get; }

		public Dictionary<INamedTypeSymbol, TypeInfo> TypeInfos { get; } = new Dictionary<INamedTypeSymbol, TypeInfo>();

		public TypeInfo GetTypeInfo(IMethodSymbol symbol, bool create = false)
		{
			return GetTypeInfo(symbol.ContainingType, create);
		}

		public TypeInfo GetTypeInfo(INamedTypeSymbol type, bool create = false)
		{
			var nestedTypes = new Stack<INamedTypeSymbol>();
			while (type != null)
			{
				nestedTypes.Push(type);
				type = type.ContainingType;
			}
			TypeInfo currentDocType = null;
			var path = Node.SyntaxTree.FilePath;
			while (nestedTypes.Count > 0)
			{
				var typeSymbol = nestedTypes.Pop().OriginalDefinition;

				if ((currentDocType?.TypeInfos ?? TypeInfos).ContainsKey(typeSymbol))
				{
					currentDocType = (currentDocType?.TypeInfos ?? TypeInfos)[typeSymbol];
					continue;
				}
				if (!create)
				{
					return null;
				}

				var location = typeSymbol.Locations.Single(o => o.SourceTree.FilePath == path);
				var node = Node.DescendantNodes()
							   .OfType<TypeDeclarationSyntax>()
							   .Single(o => o.ChildTokens().Single(t => t.IsKind(SyntaxKind.IdentifierToken)).Span ==  location.SourceSpan);
				var docType = new TypeInfo(this, typeSymbol, node);
				(currentDocType?.TypeInfos ?? TypeInfos).Add(typeSymbol, docType);
				currentDocType = docType;
			}
			return currentDocType;
		}
	}

	internal class DocumentInfo
	{
		public DocumentInfo(ProjectInfo projectInfo, Document document)
		{
			ProjectInfo = projectInfo;
			Document = document;
		}

		public ProjectInfo ProjectInfo { get; }

		public Document Document { get; }

		public IReadOnlyList<string> Folders => Document.Folders;

		public string Name => Document.Name;

		public string Path => Document.FilePath;

		public CompilationUnitSyntax RootNode { get; set; }

		public SemanticModel SemanticModel { get; set; }

		public Dictionary<INamespaceSymbol, NamespaceInfo> NamespaceInfos { get; } = new Dictionary<INamespaceSymbol, NamespaceInfo>();

		public NamespaceInfo GetNamespaceInfo(ISymbol symbol, bool create = false) 
		{
			var namespaceSymbol = symbol.ContainingNamespace;
			if (NamespaceInfos.ContainsKey(namespaceSymbol))
			{
				return NamespaceInfos[namespaceSymbol];
			}
			if (!create)
			{
				return null;
			}

			var location = namespaceSymbol.Locations.Single(o => o.SourceTree.FilePath == Path);
			var node = RootNode.DescendantNodes()
							   .OfType<NamespaceDeclarationSyntax>()
							   .Single(
								   o =>
								   {
									   var identifier = o.ChildNodes().OfType<IdentifierNameSyntax>().SingleOrDefault();
									   if (identifier != null)
									   {
										   return identifier.Span == location.SourceSpan;
									   }
									   return o.ChildNodes().OfType<QualifiedNameSyntax>().Single().Right.Span == location.SourceSpan;
								   });
			var docNamespace = new NamespaceInfo(this, namespaceSymbol, node);
			NamespaceInfos.Add(namespaceSymbol, docNamespace);
			return docNamespace;
		}

		public MethodInfo GetOrCreateMethodInfo(IMethodSymbol symbol)
		{
			return GetNamespaceInfo(symbol, true).GetTypeInfo(symbol, true).GetMethodInfo(symbol, true);
		}

		public TypeInfo GetTypeInfo(INamedTypeSymbol symbol)
		{
			return GetNamespaceInfo(symbol).GetTypeInfo(symbol);
		}

		public bool ContainsReference(IMethodSymbol symbol, ReferenceLocation reference)
		{
			return GetNamespaceInfo(symbol)?.GetTypeInfo(symbol)?.GetMethodInfo(symbol)?.References?.Contains(reference) == true;
		}

		public ISymbol GetEnclosingMethodOrPropertyOrField(ReferenceLocation reference)
		{
			var enclosingSymbol = SemanticModel.GetEnclosingSymbol(reference.Location.SourceSpan.Start);

			for (var current = enclosingSymbol; current != null; current = current.ContainingSymbol)
			{
				if (current.Kind == SymbolKind.Field)
				{
					return current;
				}

				if (current.Kind == SymbolKind.Property)
				{
					return current;
				}

				if (current.Kind == SymbolKind.Method)
				{
					var method = (IMethodSymbol)current;
					if (method.IsAccessor())
					{
						return method.AssociatedSymbol; 
					}

					if (method.MethodKind != MethodKind.AnonymousFunction)
					{
						return method;
					}
				}
			}
			// reference to a cref
			return null;
		}
	}

	internal class ProjectInfo : Dictionary<string, DocumentInfo>
	{
		public ProjectInfo(Project project)
		{
			Project = project;
		}

		public Project Project { get; }

		public async Task Analize()
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
				throw new NotSupportedException("Multiple Project for DocumentInfos");
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
				Console.WriteLine($"Symbol {symbol} is not a method and cannot be made async");
				return new SymbolInfo();
			}
			if (methodSymbol.MethodKind != MethodKind.Ordinary)
			{
				Console.WriteLine($"Method {methodSymbol} is a {methodSymbol.MethodKind} and cannot be made async");
				return new SymbolInfo();
			}

			if (methodSymbol.Parameters.Any(o => o.RefKind == RefKind.Out))
			{
				Console.WriteLine($"Method {methodSymbol} has out parameters and cannot be made async");
				return new SymbolInfo();
			}

			var baseMethods = new HashSet<IMethodSymbol>();
			if (methodSymbol.ContainingType.TypeKind == TypeKind.Interface)
			{
				baseMethods.Add(methodSymbol);
			}
			methodSymbol = methodSymbol.OriginalDefinition; // unwrap method

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
					Console.WriteLine(
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
					Console.WriteLine($"Method {methodSymbol} overrides an external method {overridenMethod} and cannot be made async");
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

			// save all overrides
			foreach (var overide in symbolInfo.Overrides)
			{
				syntax = overide.DeclaringSyntaxReferences.Single();
				doc = Project.GetDocument(syntax.SyntaxTree);
				docInfo = await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
				docInfo.GetOrCreateMethodInfo(overide);
			}

			// get and save all interface implementations
			foreach (var interfaceMethod in symbolInfo.BaseMethods)
			{
				syntax = interfaceMethod.DeclaringSyntaxReferences.Single();
				doc = Project.GetDocument(syntax.SyntaxTree);
				docInfo = await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
				docInfo.GetOrCreateMethodInfo(interfaceMethod);

				var implementations = await SymbolFinder.FindImplementationsAsync(interfaceMethod, Project.Solution).ConfigureAwait(false);
				foreach (var implementation in implementations.OfType<IMethodSymbol>())
				{
					syntax = implementation.DeclaringSyntaxReferences.Single();
					doc = Project.GetDocument(syntax.SyntaxTree);
					docInfo = await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
					docInfo.GetOrCreateMethodInfo(implementation.OriginalDefinition);
				}
			}

			// get and save all derived methods
			if (symbolInfo.OverridenMethod != null)
			{
				var overrides = await SymbolFinder.FindOverridesAsync(symbolInfo.OverridenMethod, Project.Solution).ConfigureAwait(false);
				foreach (var overide in overrides.OfType<IMethodSymbol>())
				{
					syntax = overide.DeclaringSyntaxReferences.Single();
					doc = Project.GetDocument(syntax.SyntaxTree);
					docInfo = await GetOrCreateDocumentInfo(doc).ConfigureAwait(false);
					docInfo.GetOrCreateMethodInfo(overide.OriginalDefinition);
				}
				symbolInfo.BaseMethods.Add(symbolInfo.OverridenMethod);
			}

			if (!symbolInfo.BaseMethods.Any())
			{
				symbolInfo.BaseMethods.Add(symbolInfo.MethodSymbol);
			}

			foreach (var baseMethod in symbolInfo.BaseMethods)
			{
				await GetAllReferenceLocations(await SymbolFinder.FindReferencesAsync(baseMethod, Project.Solution).ConfigureAwait(false), depth)
						.ConfigureAwait(false);
			}
		}

		private async Task GetAllReferenceLocations(IEnumerable<ReferencedSymbol> references, int depth = 0)
		{
			depth++;
			foreach (var refLocation in references.SelectMany(o => o.Locations).Where(o => !o.Location.SourceTree.FilePath.Contains(@"\Async\")))
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
				// check if we already processed the reference
				if (info.ContainsReference(symbolInfo.MethodSymbol, refLocation))
				{
					continue;
				}
				// save the reference as it can be made async
				info.GetOrCreateMethodInfo(symbolInfo.MethodSymbol).References.Add(refLocation);

				await ProcessSymbolInfo(symbolInfo, depth).ConfigureAwait(false);
			}
		}

	}

	public class AsyncLockConfiguration
	{
		public string TypeName { get; set; }

		public string MethodName { get; set; }

		public string FieldName { get; set; }

		public string Namespace { get; set; }
	}

	public class AsyncCustomTaskTypeConfiguration
	{
		public string TypeName { get; set; }

		public string Namespace { get; set; }

		public bool HasCompletedTask { get; set; }

		public bool HasFromException { get; set; }
	}

	public class AsyncConfiguration
	{
		public AsyncLockConfiguration Lock { get; set; }

		public AsyncCustomTaskTypeConfiguration CustomTaskType { get; set; } = new AsyncCustomTaskTypeConfiguration();

		public string AttributeName { get; set; }
	}

	public class WriterConfiguration
	{
		public AsyncConfiguration Async { get; set; } = new AsyncConfiguration();

	}

	public class YieldToAsyncMethodRewriter : CSharpSyntaxRewriter
	{
		private GenericNameSyntax _returnType;

		public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
		{
			_returnType = (GenericNameSyntax)node.ReturnType; // IEnumerable<T>

			var varible = LocalDeclarationStatement(
				VariableDeclaration(
					IdentifierName("var"))
					.WithVariables(
						SingletonSeparatedList(
							VariableDeclarator(
								Identifier("yields"))
								.WithInitializer(
									EqualsValueClause(
										ObjectCreationExpression(
											GenericName(
												Identifier("List"))
												.WithTypeArgumentList(
													TypeArgumentList(
														SingletonSeparatedList<TypeSyntax>(_returnType
														.TypeArgumentList
														.DescendantNodes()
														.OfType<SimpleNameSyntax>()
														.First()))))
											.WithArgumentList(
												ArgumentList()))))));

			var returnStatement = ReturnStatement(IdentifierName("yields")/*
				InvocationExpression(
					MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						IdentifierName("Task"),
						GenericName(
							Identifier("FromResult"))
							.WithTypeArgumentList(
								TypeArgumentList(SingletonSeparatedList<TypeSyntax>(_returnType)))))
					.WithArgumentList(
						ArgumentList(
							SingletonSeparatedList(
								Argument(
									IdentifierName("yields")))))*/);

			node = node.WithBody(
				Block(
					node.Body.Statements.Insert(0, varible)
						.Add(returnStatement)));
			return base.VisitMethodDeclaration(node);
		}

		public override SyntaxNode VisitYieldStatement(YieldStatementSyntax node)
		{
			var addYield = ExpressionStatement(
				InvocationExpression(
					MemberAccessExpression(
						SyntaxKind.SimpleMemberAccessExpression,
						IdentifierName("yields"),
						IdentifierName("Add")))
					.WithArgumentList(
						ArgumentList(
							SingletonSeparatedList(
								Argument(node.Expression)))));
			return addYield;
		}
	}

	public class ReturnTaskMethodRewriter : CSharpSyntaxRewriter
	{
		private TypeSyntax _returnType;
		private bool _voidMethod;
		private readonly string _completedTaskType;
		private readonly string _fromExceptionTaskType;

		public ReturnTaskMethodRewriter(AsyncCustomTaskTypeConfiguration configuration)
		{
			var configuration1 = configuration;
			_completedTaskType = configuration1.HasCompletedTask ? configuration1.TypeName : "Task";
			_fromExceptionTaskType = configuration1.HasFromException ? configuration1.TypeName : "Task";
		}

		public override SyntaxNode VisitMethodDeclaration(MethodDeclarationSyntax node)
		{
			var wrapInTryCatch = node.Body.DescendantNodes()
				.OfType<InvocationExpressionSyntax>()
				.Any(o => !o.IsAsyncMethodInvoked());
			_returnType = node.ReturnType;
			if (node.ReturnType.DescendantTokens().Any(o => o.IsKind(SyntaxKind.VoidKeyword)))
			{
				_voidMethod = true;
			}
			node = (MethodDeclarationSyntax)base.VisitMethodDeclaration(node);
			if (_voidMethod && !node.EndsWithReturnStatement())
			{
				_voidMethod = true;
				node = node.WithBody(
					node.Body.AddStatements(
						ReturnStatement(
							MemberAccessExpression(
								SyntaxKind.SimpleMemberAccessExpression,
								IdentifierName(_completedTaskType),
								IdentifierName("CompletedTask"))
							)));
			}
			return wrapInTryCatch ? node.WithBody(WrapInsideTryCatch(node.Body)) : node;
		}

		public override SyntaxNode Visit(SyntaxNode node)
		{
			var expression = node as ExpressionSyntax;
			if (expression != null && expression.IsReturned())
			{
				var invocationNode = node as InvocationExpressionSyntax;
				if (invocationNode != null && invocationNode.IsAsyncMethodInvoked())
				{
					return node;
				}
				return WrapInTaskFromResult(expression);
			}
			return base.Visit(node);
		}

		public override SyntaxNode VisitThrowStatement(ThrowStatementSyntax node)
		{
			if (node.Expression == null)
			{
				var catchNode = node.Ancestors().OfType<CatchClauseSyntax>().First();
				if (catchNode.Declaration == null)
				{
					throw new NotImplementedException("CatchClauseSyntax.Declaration == null");
				}
				return ReturnStatement(WrapInTaskFromException(IdentifierName(catchNode.Declaration.Identifier.ValueText)));
			}

			return ReturnStatement(WrapInTaskFromException(node.Expression));
		}

		private BlockSyntax WrapInsideTryCatch(BlockSyntax node)
		{
			return Block(
				SingletonList<StatementSyntax>(
					TryStatement(
						SingletonList(
							CatchClause()
								.WithDeclaration(
									CatchDeclaration(
										IdentifierName("Exception"))
										.WithIdentifier(
											Identifier("ex")))
								.WithBlock(
									Block(
										SingletonList<StatementSyntax>(
											ReturnStatement(
												InvocationExpression(
													MemberAccessExpression(
														SyntaxKind.SimpleMemberAccessExpression,
														IdentifierName(_fromExceptionTaskType),
														GenericName(
															Identifier("FromException"))
															.WithTypeArgumentList(
																TypeArgumentList(
																	SingletonSeparatedList(
																		_voidMethod
																			? PredefinedType(Token(SyntaxKind.ObjectKeyword))
																			: _returnType)))))
													.WithArgumentList(
														ArgumentList(
															SingletonSeparatedList(
																Argument(
																	IdentifierName("ex")))))))))))
						.WithBlock(node)));
		}

		private InvocationExpressionSyntax WrapInTaskFromResult(ExpressionSyntax node)
		{
			return InvocationExpression(
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName("Task"),
					GenericName(
						Identifier("FromResult"))
						.WithTypeArgumentList(
							TypeArgumentList(
								SingletonSeparatedList(
									_voidMethod
										? PredefinedType(Token(SyntaxKind.ObjectKeyword))
										: _returnType)))))
				.WithArgumentList(
					ArgumentList(
						SingletonSeparatedList(
							Argument(node))));
		}

		private InvocationExpressionSyntax WrapInTaskFromException(ExpressionSyntax node)
		{
			return InvocationExpression(
				MemberAccessExpression(
					SyntaxKind.SimpleMemberAccessExpression,
					IdentifierName(_fromExceptionTaskType),
					GenericName(
						Identifier("FromException"))
						.WithTypeArgumentList(
							TypeArgumentList(
								SingletonSeparatedList(
									_voidMethod
										? PredefinedType(Token(SyntaxKind.ObjectKeyword))
										: _returnType)))))
				.WithArgumentList(
					ArgumentList(
						SingletonSeparatedList(
							Argument(node))));
		}

	}

	internal class DocumentWriter
	{
		public DocumentWriter(DocumentInfo info, WriterConfiguration configuration)
		{
			DocumentInfo = info;
			Configuration = configuration;
			var path = "";
			for (var i = 0; i < DocumentInfo.Folders.Count; i++)
			{
				path += @"..\";
			}
			path += @"Async\";
			path += string.Join(@"\", DocumentInfo.Folders);
			DestinationFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(DocumentInfo.Path), path));
		}

		public DocumentInfo DocumentInfo { get; }

		public WriterConfiguration Configuration { get; }

		public string DestinationFolder { get; }

		private FieldDeclarationSyntax GetAsyncLockField()
		{
			var lockConfig = Configuration.Async.Lock;
			return FieldDeclaration(
				VariableDeclaration(
					IdentifierName(lockConfig.TypeName)
				)
				.WithVariables(
					SingletonSeparatedList(
						VariableDeclarator(
							Identifier(lockConfig.FieldName)
						)
						.WithInitializer(
							EqualsValueClause(
								ObjectCreationExpression(
									IdentifierName(lockConfig.TypeName)
								)
								.WithArgumentList(
									ArgumentList()
								)
							)
						)
					)
				)
			)
			.WithModifiers(
				TokenList(
					Token(SyntaxKind.PrivateKeyword),
					Token(SyntaxKind.ReadOnlyKeyword)
				)
			);
		}

		private BlockSyntax AsyncLockBlock(BlockSyntax block)
		{
			var lockConfig = Configuration.Async.Lock;
			return Block(
				SingletonList<StatementSyntax>(
					UsingStatement(block)
					.WithDeclaration(
						VariableDeclaration(
							IdentifierName("var")
						)
						.WithVariables(
							SingletonSeparatedList(
								VariableDeclarator(Identifier("releaser"))
									.WithInitializer(EqualsValueClause(
										AwaitExpression(
											InvocationExpression(
												MemberAccessExpression(
													SyntaxKind.SimpleMemberAccessExpression,
													IdentifierName(lockConfig.FieldName),
													IdentifierName(lockConfig.MethodName)
												)
											)
										)
									)
								)
							)
						)
					)
				)
			);
		}

		private MethodDeclarationSyntax RewiteMethod(MethodInfo methodInfo, MethodAnalyzeResult analyzeResult)
		{
			if (!analyzeResult.HasBody)
			{
				return methodInfo.Node
								 .WithoutAttribute("Async")
								 .ReturnAsTask(methodInfo.Symbol)
								 .WithIdentifier(Identifier(methodInfo.Node.Identifier.Value + "Async"));
			}

			var methodNode = methodInfo.Node.WithoutTrivia(); // references have spans without trivia
			foreach (var referenceResult in analyzeResult.ReferenceResults.Where(o => o.CanBeAsync))
			{
				var reference = referenceResult.ReferenceLocation;
				var startSpan = reference.Location.SourceSpan.Start - methodInfo.Node.Span.Start;
				var nameNode = methodNode.Body.DescendantNodes()
										 .OfType<SimpleNameSyntax>()
										 .First(
											 o =>
											 {
												 if (!o.IsKind(SyntaxKind.GenericName))
												 {
													 return o.Span.Start == startSpan && o.Span.Length == reference.Location.SourceSpan.Length;
												 }
												 var token = o.ChildTokens().First(t => t.IsKind(SyntaxKind.IdentifierToken));
												 return token.Span.Start == startSpan && token.Span.Length == reference.Location.SourceSpan.Length;
											 });

				if (analyzeResult.CanSkipAsync)
				{
					// return task instead of awaiting
					if (referenceResult.UsedAsReturnValue)
					{
						// modify only the method name
						methodNode = methodNode
							.ReplaceNode(nameNode, nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async")));
					}
					else
					{
						// wrap into a return statement
						var expressionStatementNode = nameNode.Ancestors().OfType<ExpressionStatementSyntax>().First();
						methodNode = methodNode
							.ReplaceNode(
								expressionStatementNode,
								ReturnStatement(
									expressionStatementNode.ReplaceNode(nameNode, nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async"))).Expression
								)
							);
					}
				}
				else
				{
					// await method
					var invocationNode = nameNode.Ancestors().OfType<InvocationExpressionSyntax>().First();
					var annotation = new SyntaxAnnotation(Guid.NewGuid().ToString());
					methodNode = methodNode
						.ReplaceNode(
							invocationNode,
							invocationNode
								.ReplaceNode(nameNode, nameNode.WithIdentifier(Identifier(nameNode.Identifier.Value + "Async")))
								.WithAdditionalAnnotations(annotation)
						);
					invocationNode = methodNode.GetAnnotatedNodes(annotation).OfType<InvocationExpressionSyntax>().Single();
					methodNode = methodNode
						.ReplaceNode(
							invocationNode,
							invocationNode.AddAwait().WithoutAnnotations(annotation)
						);
				}
			}

			if (!analyzeResult.CanBeAsnyc && !analyzeResult.IsEmpty)
			{
				var name = methodInfo.Node.TypeParameterList != null
					? GenericName(methodInfo.Node.Identifier.ValueText)
						.WithTypeArgumentList(
							TypeArgumentList(
								SeparatedList<TypeSyntax>(
									methodInfo.Node.TypeParameterList.Parameters.Select(o => IdentifierName(o.Identifier.ValueText))
									)))
					: (SimpleNameSyntax) IdentifierName(methodInfo.Node.Identifier.ValueText);

				var invocation = InvocationExpression(name)
					.WithArgumentList(
						ArgumentList(
							SeparatedList(
								methodInfo.Node.ParameterList.Parameters
										  .Select(o => Argument(IdentifierName(o.Identifier.ValueText)))
								)));
				if (methodInfo.Symbol.ReturnsVoid)
				{
					methodNode = methodNode.WithBody(Block(ExpressionStatement(invocation)));
				}
				else
				{
					methodNode = methodNode.WithBody(Block(ReturnStatement(invocation)));
				}
			}
			else if (analyzeResult.Yields)
			{
				var yieldRewriter = new YieldToAsyncMethodRewriter();
				methodNode = (MethodDeclarationSyntax) yieldRewriter.VisitMethodDeclaration(methodNode);
			}

			if(!analyzeResult.CanBeAsnyc || analyzeResult.CanSkipAsync || analyzeResult.Yields)
			{
				var taskRewriter = new ReturnTaskMethodRewriter(Configuration.Async.CustomTaskType);
				methodNode = (MethodDeclarationSyntax)taskRewriter.VisitMethodDeclaration(methodNode);
			}

			// check if method must run synhronized
			if (analyzeResult.MustRunSynchronized)
			{
				methodNode = methodNode
					.WithBody(AsyncLockBlock(methodNode.Body))
					.WithoutAttribute("MethodImpl");
			}
			methodNode = methodNode
				.WithoutAttribute("Async")
				.ReturnAsTask(methodInfo.Symbol)
				.WithIdentifier(Identifier(methodNode.Identifier.Value + "Async"));

			if (analyzeResult.MustRunSynchronized || (!analyzeResult.CanSkipAsync && !analyzeResult.Yields && analyzeResult.CanBeAsnyc))
			{
				methodNode = methodNode.AddAsync(methodNode);
			}
			return methodNode
				.WithLeadingTrivia(methodInfo.Node.GetLeadingTrivia())
				.WithTrailingTrivia(methodInfo.Node.GetTrailingTrivia());
		}

		public void Write()
		{
			var rootNode = DocumentInfo.RootNode;
			var namespaceNodes = new List<MemberDeclarationSyntax>();
			var customTask = Configuration.Async.CustomTaskType;
			var tasksUsed = false;
			foreach (var namespaceInfo in DocumentInfo.NamespaceInfos.Values.OrderBy(o => o.Node.SpanStart))
			{
				var namespaceNode = namespaceInfo.Node;
				var typeNodes = new List<MemberDeclarationSyntax>();
				foreach (var typeInfo in namespaceInfo.TypeInfos.Values.OrderBy(o => o.Node.SpanStart))
				{
					var typeNode = typeInfo.Node;
					var asyncLock = false;
					var memberNodes = new List<MemberDeclarationSyntax>();
					foreach (var methodInfo in typeInfo.MethodInfos.Values.OrderBy(o => o.Node.SpanStart))
					{
						var methodAnalyzeResult = methodInfo.Analyze();
						tasksUsed |= methodAnalyzeResult.CanSkipAsync || methodAnalyzeResult.IsEmpty || methodAnalyzeResult.Yields || !methodAnalyzeResult.CanBeAsnyc;
						/*
						if (methodAnalyzeResult.HasBody)
						{
							continue;
						}*/
						asyncLock |= methodAnalyzeResult.MustRunSynchronized;
						memberNodes.Add(RewiteMethod(methodInfo, methodAnalyzeResult));
					}
					// TODO: inifinite levels of nested types
					foreach (var subTypeInfo in typeInfo.TypeInfos.Values.OrderBy(o => o.Node.SpanStart))
					{
						var subTypeNode = subTypeInfo.Node;
						var subAsyncLock = false;
						var subMemberNodes = new List<MemberDeclarationSyntax>();
						foreach (var methodInfo in subTypeInfo.MethodInfos.Values.OrderBy(o => o.Node.SpanStart))
						{
							var methodAnalyzeResult = methodInfo.Analyze();
							tasksUsed |= methodAnalyzeResult.CanSkipAsync || methodAnalyzeResult.IsEmpty || methodAnalyzeResult.Yields || !methodAnalyzeResult.CanBeAsnyc;
							/*if (methodAnalyzeResult.HasBody)
							{
								continue;
							}*/
							subAsyncLock |= methodAnalyzeResult.MustRunSynchronized;
							subMemberNodes.Add(RewiteMethod(methodInfo, methodAnalyzeResult));
						}
						if (subAsyncLock)
						{
							var lockNamespace = Configuration.Async.Lock.Namespace;
							subMemberNodes.Insert(0, GetAsyncLockField());
							if (rootNode.Usings.All(o => o.Name.ToString() != lockNamespace))
							{
								rootNode = rootNode.AddUsings(UsingDirective(IdentifierName(lockNamespace)));
							}
						}
						subTypeNode = subTypeNode
							.AddPartial()
							.WithMembers(List(subMemberNodes));
						subTypeNode = subTypeInfo.Symbol.Locations
												.Where(o => DocumentInfo.ProjectInfo.ContainsKey(o.SourceTree.FilePath))
												.Select(o => DocumentInfo.ProjectInfo[o.SourceTree.FilePath].GetTypeInfo(subTypeInfo.Symbol).Node)
												.Any(n => n.AttributeLists.Any(o => o.Attributes.Any(a => a.Name.ToString().EndsWith("GeneratedCode"))))
							? subTypeNode.WithoutAttributes()
							: subTypeNode.AddGeneratedCodeAttribute(false);
						memberNodes.Add(subTypeNode);
					}

					// add partial to the original file
					//var partialTypeDeclaration = typeDeclaration.AddPartial();
					//if (partialTypeDeclaration != typeDeclaration)
					//{
					//	partialTypeDeclaration = partialTypeDeclaration.NormalizeWhitespace("	");
					//	rootChanged = true;
					if (asyncLock)
					{
						var lockNamespace = Configuration.Async.Lock.Namespace;
						memberNodes.Insert(0, GetAsyncLockField());
						if (rootNode.Usings.All(o => o.Name.ToString() != lockNamespace))
						{
							rootNode = rootNode.AddUsings(UsingDirective(IdentifierName(lockNamespace)));
						}
					}
					typeNode = typeNode
						.AddPartial()
						.WithMembers(List(memberNodes));
					typeNode = typeInfo.Symbol.Locations
									   .Where(o => DocumentInfo.ProjectInfo.ContainsKey(o.SourceTree.FilePath))
									   .Select(o => DocumentInfo.ProjectInfo[o.SourceTree.FilePath].GetTypeInfo(typeInfo.Symbol).Node)
									   .Any(n => n.AttributeLists.Any(o => o.Attributes.Any(a => a.Name.ToString().EndsWith("GeneratedCode"))))
						? typeNode.WithoutAttributes()
						: typeNode.AddGeneratedCodeAttribute(false);
					typeNode = typeNode.RemoveNodes(
							typeNode.DescendantNodes(descendIntoTrivia: true).OfType<DirectiveTriviaSyntax>(), SyntaxRemoveOptions.KeepNoTrivia); // remove invalid #endregion
					typeNodes.Add(typeNode);
				}
				namespaceNodes.Add(namespaceNode
						.WithMembers(List(typeNodes)));
			}

			if (rootNode.Usings.All(o => o.Name.ToString() != "System.Threading.Tasks"))
			{
				rootNode = rootNode.AddUsings(UsingDirective(IdentifierName("System.Threading.Tasks")));
			}
			if (tasksUsed && rootNode.Usings.All(o => o.Name.ToString() != "System"))
			{
				rootNode = rootNode.AddUsings(UsingDirective(IdentifierName("System")));
			}
			if (tasksUsed && !string.IsNullOrEmpty(customTask.Namespace) && rootNode.Usings.All(o => o.Name.ToString() != customTask.Namespace))
			{
				rootNode = rootNode.AddUsings(UsingDirective(IdentifierName(customTask.Namespace)));
			}

			var content = rootNode
				.WithMembers(List(namespaceNodes))
				.NormalizeWhitespace("	")
				.ToFullString();
			
			if (!Directory.Exists(DestinationFolder))
			{
				Directory.CreateDirectory(DestinationFolder);
			}
			File.WriteAllText($"{DestinationFolder}\\{DocumentInfo.Name}", content);
		}
	}




	class Program
	{
		static void Main(string[] args)
		{
			var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var nhPath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\..\NHibernate\"));


			var workspace = MSBuildWorkspace.Create();
			var project = workspace.OpenProjectAsync(Path.Combine(nhPath, "NHibernate.csproj")).Result;
			var projectInfo = new ProjectInfo(project);
			AsyncContext.Run(() => projectInfo.Analize());

			var configuration = new WriterConfiguration
			{
				Async = new AsyncConfiguration
				{
					Lock = new AsyncLockConfiguration
					{
						TypeName = "AsyncLock",
						MethodName = "LockAsync",
						FieldName = "_lock",
						Namespace = "NHibernate.Util"
					},
					CustomTaskType = new AsyncCustomTaskTypeConfiguration
					{
						HasFromException = true,
						HasCompletedTask = true,
						TypeName = "TaskHelper",
						Namespace = "NHibernate.Util"
					},
					AttributeName = "Async"
				}
			};

			foreach (var pair in projectInfo.Where(o => o.Value.NamespaceInfos.Any()))
			{
				var writer = new DocumentWriter(pair.Value, configuration);
				writer.Write();
			}
		}
	}
}
