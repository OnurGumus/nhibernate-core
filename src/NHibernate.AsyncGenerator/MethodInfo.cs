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
		private bool _ignoreCalculated;
		private bool _ignoreCalculating;

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

		public HashSet<MethodInfo> InvokedBy { get; } = new HashSet<MethodInfo>();

		/// <summary>
		/// Implementation/derived/base/interface methods
		/// </summary>
		public HashSet<MethodInfo> RelatedMethods { get; } = new HashSet<MethodInfo>();

		/// <summary>
		/// Related and invoked by methods
		/// </summary>
		public IEnumerable<MethodInfo> Dependencies
		{
			get { return InvokedBy.Union(RelatedMethods); }
		}

		/// <summary>
		/// External Base/derivered or interface/implementation methods
		/// </summary>
		public HashSet<IMethodSymbol> ExternalRelatedMethods { get; } = new HashSet<IMethodSymbol>();

		// async methods that have the same parameters and name and are external
		public HashSet<IMethodSymbol> ExternalAsyncMethods { get; } = new HashSet<IMethodSymbol>();

		/// <summary>
		/// When true the method will not be generated. Example would be a test method (never invoked in code) that does not have any async calls 
		/// or a method that is only called by that test method. If there are some references (methods invoked) we need to check them as they can be
		/// 
		/// Steps:
		/// 1. Check if the method is missing or implements an abstract method
		///   a) If true then we cannot ignore this method
		/// 3. Calculate the Ignore property for all dependant methods
		///   a) If there is a method that is not ignored then this wont be too
		///     i) Calculate ignore property for all current references
		/// 2. Calculate the Ignore property for all current references
		/// </summary>
		public void CalculateIgnore(int deep = 0, HashSet<MethodInfo> processedMethodInfos = null)
		{
			if (_ignoreCalculated)
			{
				return;
			}
			if (_ignoreCalculating)
			{
				throw new Exception("_ignoreCalculating");
			}
			_ignoreCalculating = true;


			if (processedMethodInfos == null)
			{
				processedMethodInfos = new HashSet<MethodInfo> {this};
			}
			else
			{
				processedMethodInfos.Add(this);
			}

			deep++;
			if (deep > 100)
			{
				foreach (var refResult in ReferenceResults)
				{
					refResult.CalculateIgnore(deep, processedMethodInfos);
				}
				_ignoreCalculating = false;
				_ignoreCalculated = true;
				return;
			}

			if (Missing || ImplementsAbstractMethod)
			{
				foreach (var refResult in ReferenceResults)
				{
					refResult.CalculateIgnore(deep, processedMethodInfos);
				}
				_ignoreCalculating = false;
				_ignoreCalculated = true;
				return;
			}

			// Calculate the ignore property for all dependent methods. 
			// If any of above methods is required then this will be too
			foreach (var methodInfo in Dependencies)
			{
				if (methodInfo == this)
				{
					continue;
				}
				// If we detect a circular dependency we assume that the method is required
				if (methodInfo._ignoreCalculating)
				{
					foreach (var refResult in ReferenceResults)
					{
						refResult.CalculateIgnore(deep, processedMethodInfos);
					}
					_ignoreCalculating = false;
					_ignoreCalculated = true;
					return;
				}

				methodInfo.CalculateIgnore(deep, processedMethodInfos);
				if (methodInfo.Ignore)
				{
					continue;
				}
				foreach (var refResult in ReferenceResults)
				{
					refResult.CalculateIgnore(deep, processedMethodInfos);
				}
				_ignoreCalculating = false;
				_ignoreCalculated = true;
				return;
			}

			var dependencies = ReferenceResults
				.Union(GetAllRelatedMethods().ToList().SelectMany(o => o.ReferenceResults))
				.ToList();

			foreach (var refResult in dependencies)
			{
				// if the reference cannot be async we should preserve the original method. TODO: what if the method has dependecies that are async
				if (!refResult.CanBeAsync && refResult.MethodInfo != null)
				{
					refResult.MethodInfo.CanBeAsnyc = false;
				}
				refResult.CalculateIgnore(deep, processedMethodInfos);
			}
			Ignore = dependencies.All(o => o.Ignore);
			_ignoreCalculating = false;
			_ignoreCalculated = true;
		}

		public bool HasReferences
		{
			get
			{
				return References.Any() || GetAllRelatedMethods().Any(o => o.References.Any());
			}
		}

		public bool HasRequiredExternalMethods(int deep = 0, HashSet<MethodInfo> processedMethodInfos = null)
		{
			deep++;
			if (deep > 980)
			{
				return true;
			}
			if (processedMethodInfos == null)
			{
				processedMethodInfos = new HashSet<MethodInfo> { this };
			}

			if (ExternalRelatedMethods.Select(o => o.ContainingType.GetFullName())
									.Except(ExternalAsyncMethods.Select(o => o.ContainingType.GetFullName()))
									.Any())
			{
				return true;
			}
			foreach (var depednency in GetAllRelatedMethods())
			{
				if (processedMethodInfos.Contains(depednency))
				{
					continue;
				}
				processedMethodInfos.Add(depednency);
				if (depednency.HasRequiredExternalMethods(deep, processedMethodInfos))
				{
					return true;
				}
			}
			return false;
		}

		public bool ImplementsAbstractMethod
		{
			get { return OverridenMethod != null && OverridenMethod.IsAbstract; }
		}

		public IMethodSymbol OverridenMethod { get; set; }

		/// <summary>
		/// References to other methods that are invoked inside this method and are candidates to be async
		/// </summary>
		internal HashSet<ReferenceLocation> References { get; } = new HashSet<ReferenceLocation>();

		/// <summary>
		/// Type references that need to be renamed
		/// </summary>
		public HashSet<ReferenceLocation> TypeReferences { get; } = new HashSet<ReferenceLocation>();

		public IEnumerable<MethodInfo> GetAllRelatedMethods()
		{
			var deps = new HashSet<MethodInfo>();
			var depsQueue = new Queue<MethodInfo>(RelatedMethods);
			while (depsQueue.Count > 0)
			{
				var dependency = depsQueue.Dequeue();
				if (deps.Contains(dependency))
				{
					continue;
				}
				deps.Add(dependency);
				foreach (var subDependency in dependency.RelatedMethods)
				{
					if (!deps.Contains(subDependency))
					{
						depsQueue.Enqueue(subDependency);
					}
				}
				yield return dependency;
			}
		}

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
				foreach (var subDependency in dependency.Dependencies)
				{
					if (!deps.Contains(subDependency))
					{
						depsQueue.Enqueue(subDependency);
					}
				}
				yield return dependency;
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
			var asyncMethodSymbol = docInfo.ProjectInfo.MethodAsyncConterparts.ContainsKey(methodSymbol.OriginalDefinition)
				? docInfo.ProjectInfo.MethodAsyncConterparts[methodSymbol.OriginalDefinition]
				: null;

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
			var docInfo = TypeInfo.NamespaceInfo.DocumentInfo;
			var config = docInfo.ProjectInfo.Configuration;
			var methodSymbol = (IMethodSymbol) docInfo.SemanticModel.GetSymbolInfo(nameNode).Symbol;

			MethodInfo methodInfo = null;
			var methodDocInfo = methodSymbol.DeclaringSyntaxReferences.Select(o => docInfo.ProjectInfo.GetDocumentInfo(o))
						.SingleOrDefault(o => o != null);
			if (methodDocInfo != null)
			{
				methodInfo = methodDocInfo.GetMethodInfo(methodSymbol);
			}
			var result = new MethodReferenceResult(reference, nameNode, methodSymbol, methodInfo)
			{
				CanBeAsync = true,
				DeclaredWithinSameType = TypeInfo.Symbol.GetFullName() == methodSymbol.ContainingType.GetFullName()
			};
			if (!config.CanConvertReferenceFunc(methodSymbol))
			{
				result.UserIgnore = true;
				return result;
			}

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
					var config = TypeInfo.NamespaceInfo.DocumentInfo.ProjectInfo.Configuration;
					if (config.FindAsyncCounterpart != null)
					{
						asyncMethodSymbol = config.FindAsyncCounterpart.Invoke(methodSymbol.OriginalDefinition);
					}
					else
					{
						asyncMethodSymbol = methodSymbol.ContainingType.EnumerateBaseTypesAndSelf()
														.SelectMany(o => o.GetMembers(methodSymbol.Name + "Async"))
														.OfType<IMethodSymbol>()
														.Where(o => o.TypeParameters.Length == methodSymbol.TypeParameters.Length)
														.FirstOrDefault(o => o.HaveSameParameters(methodSymbol));
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


		#region Analyze properties

		public bool CanBeAsnyc { get; private set; }
		/*
		public bool CanBeAsnyc(int deep = 0, HashSet<MethodInfo> processedMethodInfos = null)
		{
			if (processedMethodInfos == null)
			{
				processedMethodInfos = new HashSet<MethodInfo>();
			}
			processedMethodInfos.Add(this);

			deep++;
			if (deep > 980)
			{
				return false;
			}
			return ReferenceResults.Where(o => o.MethodInfo == null || !processedMethodInfos.Contains(o.MethodInfo))
				.All(o => !o.CalculateIgnore(deep, processedMethodInfos));
		}*/

		public HashSet<MethodReferenceResult> ReferenceResults { get; } = new HashSet<MethodReferenceResult>();

		public bool CanSkipAsync { get; internal set; }

		public bool MustRunSynchronized { get; internal set; }

		public bool HasBody => Node.Body != null;

		public bool HasYields { get; internal set; }

		public bool IsEmpty { get; internal set; }

		/// <summary>
		/// Used for a method that is not supposed to be async at the moment of creation, 
		/// it will be removed from the TypeInfo on post analyize if the calculation of ignore will be true
		/// </summary>
		public bool LazyCreate { get; internal set; }

		#endregion

		#region Post Analyze properties

		public bool Ignore { get; private set; }

		#endregion

		public void PostAnalyze()
		{
			CalculateIgnore();
			if (Ignore && LazyCreate)
			{
				TypeInfo.MethodInfos.Remove(Node);
			}

			// Remove all references as the method wont be transformed to asnyc
			if (!CanBeAsnyc && !RelatedMethods.Any() && !Missing)
			{
				foreach (var invoked in InvokedBy)
				{
					var toRemove = invoked.ReferenceResults.Where(o => o.MethodInfo == this).ToList();
					while (toRemove.Count > 0)
					{
						invoked.ReferenceResults.Remove(toRemove[0]);
						toRemove.RemoveAt(0);
					}
				}
			}
		}

		// check if all the references can be converted to async
		public void Analyze()
		{
			var solutionConfig = TypeInfo.NamespaceInfo.DocumentInfo.ProjectInfo.SolutionInfo.Configuration;

			foreach (var reference in References)
			{
				ReferenceResults.Add(AnalyzeReference(reference));
			}
			CanBeAsnyc = ReferenceResults.Any(o => o.CanBeAsync);

			// TODO: check if this is correct
			if (TypeInfo.TypeTransformation == TypeTransformation.Partial && GetAllRelatedMethods().ToList().Any(o => o.InvokedBy.Any()))
			{
				Missing = true;
			}

			var counter = new MethodStatementsCounterVisitior();
			counter.Visit(Node.Body);

			if (counter.TotalYields > 0)
			{
				HasYields = true;
			}
			if (counter.TotalStatements == 0)
			{
				IsEmpty = true;
			}

			if (Node.AttributeLists
					.SelectMany(o => o.Attributes.Where(a => a.Name.ToString() == "MethodImpl"))
					.Any(o => o.ArgumentList.Arguments.Any(a => a.Expression.ToString() == "MethodImplOptions.Synchronized")))
			{
				MustRunSynchronized = true;
			}
			else if (counter.InvocationExpressions.Count == 1 && Symbol.GetAttributes()
				.All(o => !solutionConfig.TestAttributeNames.Contains(o.AttributeClass.Name)))
			{
				var invocation = counter.InvocationExpressions[0];
				if (Symbol.ReturnsVoid)
				{
					// if there is only one await and it is the last statement to be executed we can just forward the task
					if (ReferenceResults.All(o => o.LastStatement) &&
						invocation.Parent.IsKind(SyntaxKind.ExpressionStatement) &&
						counter.TotalStatements == 1)
					{
						CanSkipAsync = true;
					}
				}
				else
				{
					// if there is only one await and it is used as a reutorn value we can just forward the task
					if (ReferenceResults.All(o => o.UsedAsReturnValue) &&
						invocation.IsReturned() &&
						counter.TotalStatements == 1)
					{
						CanSkipAsync = true;
					}
				}
			}
		}

	}
}
