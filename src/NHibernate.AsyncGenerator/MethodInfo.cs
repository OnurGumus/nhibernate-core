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
					Logger.Warn($"Cannot await async method in a select clause:\r\n{invocationNode.Parent}\r\n");
				}
				else
				{
					var anonFunctionNode = nameNode.Ancestors().OfType<AnonymousFunctionExpressionSyntax>().FirstOrDefault();
					if (anonFunctionNode?.AsyncKeyword.IsMissing == false)
					{
						result.CanBeAsync = false;
						Logger.Warn($"Cannot await async method in an non async anonymous function:\r\n{anonFunctionNode}\r\n");
					}
				}

			}
			else if (nameNode.Parent.IsKind(SyntaxKind.Argument)) // method is passed as an argument check if the types matches
			{
				var methodArgTypeInfo = ModelExtensions.GetTypeInfo(docInfo.SemanticModel, nameNode);
				if (methodArgTypeInfo.ConvertedType?.TypeKind == TypeKind.Delegate)
				{
					var delegateMethod = (IMethodSymbol)methodArgTypeInfo.ConvertedType.GetMembers("Invoke").First();

					if (!delegateMethod.IsAsync)
					{
						result.CanBeAsync = false;
						Logger.Warn($"Cannot pass an async method as parameter to a non async Delegate method:\r\n{delegateMethod}\r\n");
					}
					else
					{
						result.PassedAsArgument = true;
						var argumentMethodSymbol = (IMethodSymbol)ModelExtensions.GetSymbolInfo(docInfo.SemanticModel, nameNode).Symbol;
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
			else if (nameNode.Parent.IsKind(SyntaxKind.AddAssignmentExpression)) // method attached to an event
			{
				result.CanBeAsync = false;
				Logger.Warn($"Cannot attach an async method to an event (void async is not an option as cannot be awaited):\r\n{nameNode.Parent}\r\n");
			}
			else if (nameNode.Parent.IsKind(SyntaxKind.EqualsValueClause)) // method assigned to a variable
			{
				result.CanBeAsync = false;
				Logger.Warn($"Assigning async method to a variable is not supported:\r\n{nameNode.Parent}\r\n");
			}
			else
			{
				throw new NotSupportedException("Unknown method usage");
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
