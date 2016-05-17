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

		public HashSet<ReferenceLocation> References { get; } = new HashSet<ReferenceLocation>();

		public HashSet<ReferenceLocation> BodyToAsyncMethodsReferences { get; } = new HashSet<ReferenceLocation>();

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

			var anonFunctionNode = node.Ancestors()
				.OfType<AnonymousFunctionExpressionSyntax>()
				.FirstOrDefault();
			if (anonFunctionNode?.AsyncKeyword.IsMissing == false)
			{
				// Custom code
				var docInfo = TypeInfo.NamespaceInfo.DocumentInfo;
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

		public MethodReferenceResult AnalyzeReference(ReferenceLocation reference)
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
			var result = new MethodReferenceResult(reference, nameNode)
			{
				CanBeAsync = true
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

		public List<AsyncCounterpartMethod> FindAsyncCounterpartMethodsWhitinBody()
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
				var methodInfo = ModelExtensions.GetSymbolInfo(semanticModel, invocation.Expression).Symbol as IMethodSymbol;
				if (methodInfo == null)
				{
					continue;
				}
				var type = methodInfo.ContainingType;
				var asyncMethodInfo = type.GetMembers(methodInfo.Name + "Async")
										  .OfType<IMethodSymbol>()
										  .FirstOrDefault(o => o.HaveSameParameters(methodInfo));
				if (asyncMethodInfo == null)
				{
					continue;
				}
				result.Add(new AsyncCounterpartMethod
				{
					MethodSymbol = methodInfo,
					AsyncMethodSymbol = asyncMethodInfo,
					MethodNode = invocation.Expression
				});
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

			foreach (var reference in References)
			{
				result.ReferenceResults.Add(AnalyzeReference(reference));
			}

			foreach (var reference in BodyToAsyncMethodsReferences)
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

			var solutionConfig = TypeInfo.NamespaceInfo.DocumentInfo.ProjectInfo.SolutionInfo.Configuration;
			if (Node.AttributeLists
					.SelectMany(o => o.Attributes.Where(a => a.Name.ToString() == "MethodImpl"))
					.Any(o => o.ArgumentList.Arguments.Any(a => a.Expression.ToString() == "MethodImplOptions.Synchronized")))
			{
				result.MustRunSynchronized = true;
			}
			else if (counter.InvocationExpressions.Count == 1 && Symbol.GetAttributes()
				.All(o => !solutionConfig.TestAttributeNames.Contains(o.AttributeClass.Name)))
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
}
