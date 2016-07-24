using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
	public class MethodInfo
	{
		private static readonly ILog Logger = LogManager.GetLogger(typeof(MethodInfo));

		public MethodInfo(TypeInfo typeInfo, IMethodSymbol symbol, MethodDeclarationSyntax node)
		{
			TypeInfo = typeInfo;
			Symbol = symbol;
			Node = node;
		}

		public TypeInfo TypeInfo { get; }

		public IMethodSymbol Symbol { get; }

		public MethodDeclarationSyntax Node { get; }

		public bool Missing { get; set; }

		public HashSet<MethodInfo> Dependencies { get; } = new HashSet<MethodInfo>();

		public HashSet<IMethodSymbol> ExternalDependencies { get; } = new HashSet<IMethodSymbol>();

		/// <summary>
		/// When true the method will be ignored when generating async members
		/// </summary>
		public bool Ignore
		{
			get
			{
				return !Missing && !References.Any() && !TypeReferences.Any() && GetAllDependencies().All(o => !o.References.Any());
			}
		}

		public bool HasReferences
		{
			get
			{
				return References.Any() || GetAllDependencies().Any(o => o.References.Any());
			}
		}

		public bool IsRequired
		{
			get { return ExternalDependencies.Any() || GetAllDependencies().Any(o => o.ExternalDependencies.Any())/* || Dependencies.Any(o => o.Ignore)*/; }
		}

		public bool Candidate { get; set; }

		public IMethodSymbol OverridenMethod { get; set; }

		/// <summary>
		/// references that need to be async
		/// </summary>
		public HashSet<ReferenceLocation> References { get; } = new HashSet<ReferenceLocation>();

		/// <summary>
		/// Type references that need to be renamed
		/// </summary>
		public HashSet<ReferenceLocation> TypeReferences { get; } = new HashSet<ReferenceLocation>();

		//public HashSet<ReferenceLocation> BodyToAsyncMethodsReferences { get; } = new HashSet<ReferenceLocation>();

		public IEnumerable<MethodInfo> GetAllDependencies()
		{
			var deps = new HashSet<MethodInfo>();
			var depsQueue = new Queue<MethodInfo>(Dependencies);
			while (depsQueue.Count > 0)
			{
				var dependency = depsQueue.Dequeue();
				if (deps.Contains(dependency))
				{
					continue;
				}
				deps.Add(dependency);
				yield return dependency;
				foreach (var subDependency in dependency.Dependencies.Where(o => !deps.Contains(o)))
				{
					depsQueue.Enqueue(subDependency);
				}
			}
		}

		private void AnalyzeInvocationExpression(SyntaxNode node, SimpleNameSyntax nameNode, MethodReferenceResult result)
		{
			var selectClause = node.Ancestors()
				.FirstOrDefault(o => o.IsKind(SyntaxKind.SelectClause));
			if (selectClause != null) // await is not supported in select clause
			{
				result.CanBeAsync = false;
				Logger.Warn($"Cannot await async method in a select clause:\r\n{selectClause}\r\n");
				return;
			}
			var docInfo = TypeInfo.NamespaceInfo.DocumentInfo;

			var methodSymbol = (IMethodSymbol)docInfo.SemanticModel.GetSymbolInfo(nameNode).Symbol;
			var asyncMethodSymbol = docInfo.ProjectInfo.MethodAsyncConterparts[methodSymbol.OriginalDefinition];

			if (asyncMethodSymbol != null && (asyncMethodSymbol.ReturnsVoid || asyncMethodSymbol.ReturnType.Name != "Task"))
			{
				result.CanBeAwaited = false;
				//Logger.Warn($"Cannot await method that is either void or do not return a Task:\r\n{methodSymbol}\r\n");

				// check if the invocation expression takes any func as a parameter, we will allow to rename the method only if there is an awaitable invocation
				var delegateParams = methodSymbol.Parameters
												 .Select((o, i) => new {o.Type, Index = i})
												 .Where(o => o.Type.TypeKind == TypeKind.Delegate)
												 .ToList();
				var invocationNode = (InvocationExpressionSyntax) node;
				var delegateParamNodes = invocationNode.ArgumentList.Arguments
											   .Select((o, i) => new {o.Expression, Index = i})
											   .Where(o => delegateParams.Any(p => p.Index == o.Index))
											   .Where(o => References.Any(r => o.Expression.Span.Contains(r.Location.SourceSpan)))
											   .ToList();
				if (!delegateParamNodes.Any())
				{
					result.CanBeAsync = false;
					Logger.Warn($"Cannot convert method to async as it is either void or do not return a Task and has not any parameters that can be async:\r\n{methodSymbol}\r\n");
				}
				return;
			}

			var anonFunctionNode = node.Ancestors()
				.OfType<AnonymousFunctionExpressionSyntax>()
				.FirstOrDefault();
			if (anonFunctionNode?.AsyncKeyword.IsMissing == false)
			{
				// Custom code
				
				var methodArgTypeInfo = ModelExtensions.GetTypeInfo(docInfo.SemanticModel, anonFunctionNode);
				var convertedType = methodArgTypeInfo.ConvertedType;
				if (convertedType != null && convertedType.ContainingAssembly.Name == "nunit.framework" && 
					(
						convertedType.Name == "TestDelegate" ||
						convertedType.Name == "ActualValueDelegate"
					))
				{
					result.MakeAnonymousFunctionAsync = true;
					return;
				}
				//end

				result.CanBeAsync = false;
				Logger.Warn($"Cannot await async method in an non async anonymous function:\r\n{anonFunctionNode}\r\n");
			}
		}

		private void AnalyzeArgumentExpression(SyntaxNode node, SimpleNameSyntax nameNode, MethodReferenceResult result)
		{
			result.PassedAsArgument = true;
			var docInfo = TypeInfo.NamespaceInfo.DocumentInfo;
			var methodArgTypeInfo = ModelExtensions.GetTypeInfo(docInfo.SemanticModel, nameNode);
			if (methodArgTypeInfo.ConvertedType?.TypeKind != TypeKind.Delegate)
			{
				return;
			}

			// Custom code
			var convertedType = methodArgTypeInfo.ConvertedType;
			if (convertedType.ContainingAssembly.Name == "nunit.framework" && convertedType.Name == "TestDelegate")
			{
				result.WrapInsideAsyncFunction = true;
				return;
			}
			//end

			var delegateMethod = (IMethodSymbol)methodArgTypeInfo.ConvertedType.GetMembers("Invoke").First();

			if (!delegateMethod.IsAsync)
			{
				result.CanBeAsync = false;
				Logger.Warn($"Cannot pass an async method as parameter to a non async Delegate method:\r\n{delegateMethod}\r\n");
			}
			else
			{
				var argumentMethodSymbol = (IMethodSymbol)ModelExtensions.GetSymbolInfo(docInfo.SemanticModel, nameNode).Symbol;
				if (!argumentMethodSymbol.ReturnType.Equals(delegateMethod.ReturnType)) // i.e IList<T> -> IEnumerable<T>
				{
					result.MustBeAwaited = true;
				}
			}
		}

		public MethodReferenceResult AnalyzeMethodReference(ReferenceLocation reference)
		{
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
			var docInfo = TypeInfo.NamespaceInfo.DocumentInfo;
			var methodSymbol = (IMethodSymbol) docInfo.SemanticModel.GetSymbolInfo(nameNode).Symbol;
			var result = new MethodReferenceResult(reference, nameNode, methodSymbol)
			{
				CanBeAsync = true,
				DeclaredWithinSameType = TypeInfo.Symbol.GetFullName() == methodSymbol.ContainingType.GetFullName()
			};

			var currNode = nameNode.Parent;
			var ascend = true;
			while (ascend)
			{
				ascend = false;
				switch (currNode.Kind())
				{
					case SyntaxKind.ConditionalExpression:
						result.InsideCondition = true;
						break;
					case SyntaxKind.InvocationExpression:
						AnalyzeInvocationExpression(currNode, nameNode, result);
						break;
					case SyntaxKind.Argument:
						AnalyzeArgumentExpression(currNode, nameNode, result);
						break;
					case SyntaxKind.AddAssignmentExpression:
						result.CanBeAsync = false;
						Logger.Warn($"Cannot attach an async method to an event (void async is not an option as cannot be awaited):\r\n{nameNode.Parent}\r\n");
						break;
					case SyntaxKind.VariableDeclaration:
						result.CanBeAsync = false;
						Logger.Warn($"Assigning async method to a variable is not supported:\r\n{nameNode.Parent}\r\n");
						break;
					case SyntaxKind.CastExpression:
						result.MustBeAwaited = true;
						ascend = true;
						break;
					case SyntaxKind.ReturnStatement:
						break;
					// skip
					case SyntaxKind.VariableDeclarator:
					case SyntaxKind.EqualsValueClause:
					case SyntaxKind.SimpleMemberAccessExpression:
					case SyntaxKind.ArgumentList:
					case SyntaxKind.ObjectCreationExpression:
						ascend = true;
						break;
					default:
						throw new NotSupportedException($"Unknown node kind: {currNode.Kind()}");
				}

				if (ascend)
				{
					currNode = currNode.Parent;
				}
			}

			var statementNode = currNode.AncestorsAndSelf().OfType<StatementSyntax>().First();
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
			return result;
		}

		public List<AsyncCounterpartMethod> FindAsyncCounterpartMethodsWhitinBody(Dictionary<IMethodSymbol, IMethodSymbol> methodAsyncConterparts = null)
		{
			var result = new List<AsyncCounterpartMethod>();
			if (Node.Body == null)
			{
				return result;
			}

			var semanticModel = TypeInfo.NamespaceInfo.DocumentInfo.SemanticModel;
			foreach (var invocation in Node.Body.DescendantNodes()
										   .OfType<InvocationExpressionSyntax>())
			{
				var methodSymbol = ModelExtensions.GetSymbolInfo(semanticModel, invocation.Expression).Symbol as IMethodSymbol;
				if (methodSymbol == null)
				{
					continue;
				}
				IMethodSymbol asyncMethodSymbol;
				if (methodAsyncConterparts != null && methodAsyncConterparts.ContainsKey(methodSymbol.OriginalDefinition) )
				{
					asyncMethodSymbol = methodAsyncConterparts[methodSymbol.OriginalDefinition];
				}
				else
				{
					var type = methodSymbol.ContainingType;
					asyncMethodSymbol = type.GetMembers(methodSymbol.Name + "Async")
											  .OfType<IMethodSymbol>()
											  .FirstOrDefault(o => o.HaveSameParameters(methodSymbol));
					if (asyncMethodSymbol == null)
					{
						var config = TypeInfo.NamespaceInfo.DocumentInfo.ProjectInfo.Configuration;
						asyncMethodSymbol = config.FindAsyncCounterpart?.Invoke(methodSymbol);
					}
					methodAsyncConterparts?.Add(methodSymbol.OriginalDefinition, asyncMethodSymbol?.OriginalDefinition);
				}
				if (asyncMethodSymbol == null)
				{
					continue;
				}

				result.Add(new AsyncCounterpartMethod
				{
					MethodSymbol = methodSymbol.OriginalDefinition,
					AsyncMethodSymbol = asyncMethodSymbol.OriginalDefinition,
					MethodNode = invocation.Expression
				});
			}
			return result;
		}

		public MethodAnalyzeResult PostAnalyzeResult { get; private set; }

		// check if all the references can be converted to async
		public void PostAnalyze()
		{
			if (Node.Body == null)
			{
				PostAnalyzeResult = new MethodAnalyzeResult();
				return;
			}
			PostAnalyzeResult = new MethodAnalyzeResult
			{
				HasBody = true
			};

			foreach (var reference in References)
			{
				PostAnalyzeResult.ReferenceResults.Add(AnalyzeMethodReference(reference));
			}
			/*
			if (Candidate)
			{
				var syntax = OverridenMethod.DeclaringSyntaxReferences.Single();
				var docInfo = TypeInfo.NamespaceInfo.DocumentInfo.ProjectInfo.GetDocumentInfo(syntax);
				if (docInfo == null)
				{
					result.IsValid = false;
					return result;
				}
				var methodInfo = docInfo.GetMethodInfo(OverridenMethod);
				if (methodInfo == null)
				{
					result.IsValid = false;
					return result;
				}
			}*/


			var counter = new MethodStatementsCounterVisitior();
			counter.Visit(Node.Body);

			if (counter.TotalYields > 0)
			{
				PostAnalyzeResult.Yields = true;
			}
			if (counter.TotalStatements == 0)
			{
				PostAnalyzeResult.IsEmpty = true;
			}
			if (PostAnalyzeResult.ReferenceResults.Any(o => o.CanBeAsync))
			{
				PostAnalyzeResult.CanBeAsnyc = true;
			}

			var solutionConfig = TypeInfo.NamespaceInfo.DocumentInfo.ProjectInfo.SolutionInfo.Configuration;
			if (Node.AttributeLists
					.SelectMany(o => o.Attributes.Where(a => a.Name.ToString() == "MethodImpl"))
					.Any(o => o.ArgumentList.Arguments.Any(a => a.Expression.ToString() == "MethodImplOptions.Synchronized")))
			{
				PostAnalyzeResult.MustRunSynchronized = true;
			}
			else if (counter.InvocationExpressions.Count == 1 && Symbol.GetAttributes()
				.All(o => !solutionConfig.TestAttributeNames.Contains(o.AttributeClass.Name)))
			{
				var invocation = counter.InvocationExpressions[0];
				if (Symbol.ReturnsVoid)
				{
					// if there is only one await and it is the last statement to be executed we can just forward the task
					if (PostAnalyzeResult.ReferenceResults.All(o => o.LastStatement) &&
						invocation.Parent.IsKind(SyntaxKind.ExpressionStatement) &&
						counter.TotalStatements == 1)
					{
						PostAnalyzeResult.CanSkipAsync = true;
					}
				}
				else
				{
					// if there is only one await and it is used as a reutorn value we can just forward the task
					if (PostAnalyzeResult.ReferenceResults.All(o => o.UsedAsReturnValue) &&
						invocation.IsReturned() &&
						counter.TotalStatements == 1)
					{
						PostAnalyzeResult.CanSkipAsync = true;
					}
				}
			}
		}
	}
}
