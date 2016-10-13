using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;
using Microsoft.CodeAnalysis.Text;
using NHibernate.AsyncGenerator.Extensions;
using Nito.AsyncEx;


[assembly: log4net.Config.XmlConfigurator]
namespace NHibernate.AsyncGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var basePath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\..\"));

			Func<Project, IMethodSymbol, bool, Task<IMethodSymbol>> findAsyncFn = async (project, symbol, inherit) =>
			{
				var ns = symbol.ContainingNamespace?.ToString() ?? "";
				var isToList = project.Name != "NHibernate" && symbol.ContainingType.Name == "Enumerable" && symbol.Name == "ToList";
				if ((isToList || symbol.ContainingType.Name == "Queryable") && ns.StartsWith("System.Linq"))
				{
					var  nhProject = project.Solution.Projects.First(o => o.Name == "NHibernate");
					var doc = nhProject.Documents.First(o => o.Name == "LinqExtensionMethods.cs");
					var rootNode = await doc.GetSyntaxRootAsync().ConfigureAwait(false);
					var semanticModel = await doc.GetSemanticModelAsync().ConfigureAwait(false);
					var candidateNodes = rootNode.DescendantNodes()
										.OfType<MethodDeclarationSyntax>()
										.Where(
											o =>
												o.Identifier.ValueText == symbol.Name + "Async" &&
												o.ParameterList.Parameters.Count - 1 == symbol.Parameters.Length &&
												(o.TypeParameterList?.Parameters.Count ?? 0) == symbol.TypeParameters.Length)
										.ToList();
					foreach (var candidateNode in candidateNodes)
					{
						var candidateSymbol = semanticModel.GetDeclaredSymbol(candidateNode);
						ITypeSymbol symbolType;
						ITypeSymbol candidateType;

						// For average we need to check the expression type argument
						if (symbol.Name == "Average")
						{
							if (!symbol.Parameters.Any())
							{
								symbolType = ((INamedTypeSymbol)symbol.ReceiverType).TypeArguments.First();
								candidateType = ((INamedTypeSymbol) candidateSymbol.Parameters.First().Type).TypeArguments.First();
							}
							else
							{
								symbolType = ((INamedTypeSymbol)((INamedTypeSymbol)symbol.Parameters.First().Type).TypeArguments.First())
								.TypeArguments.Last();
								candidateType = ((INamedTypeSymbol)((INamedTypeSymbol)candidateSymbol.Parameters.Last().Type).TypeArguments.First())
									.TypeArguments.Last();
							}
						}
						else
						{
							symbolType = symbol.ReturnType;
							candidateType = ((INamedTypeSymbol)candidateSymbol.ReturnType).TypeArguments.First();
						}
						if (symbolType.ToString() == candidateType.ToString())
						{
							return candidateSymbol;
						}

					}
				}

				if (ns.StartsWith("System") && !ns.StartsWith("System.Data"))
				{
					return null;
				}

				Func<IParameterSymbol, IParameterSymbol, bool> paramCompareFunc = null;
				if (symbol.ContainingAssembly.Name == "nunit.framework" && symbol.ContainingType.Name == "Assert")
				{
					var delegateNames = new HashSet<string> { "AsyncTestDelegate", "TestDelegate" };
					paramCompareFunc = (p1, p2) => p1.Type.Equals(p2.Type) ||
												   (delegateNames.Contains(p1.Type.Name) && delegateNames.Contains(p2.Type.Name));
				}

				if (inherit)
				{
					return symbol.ContainingType.EnumerateBaseTypesAndSelf()
								 .SelectMany(o => o.GetMembers(symbol.Name + "Async"))
								 .OfType<IMethodSymbol>()
								 .Where(o => o.TypeParameters.Length == symbol.TypeParameters.Length)
								 .FirstOrDefault(o => o.HaveSameParameters(symbol, paramCompareFunc));
				}
				return symbol.ContainingType.GetMembers(symbol.Name + "Async")
							 .OfType<IMethodSymbol>()
							 .Where(o => o.TypeParameters.Length == symbol.TypeParameters.Length)
							 .FirstOrDefault(o => o.HaveSameParameters(symbol, paramCompareFunc));
			};

			var solutionConfig = new SolutionConfiguration(Path.Combine(basePath, @"NHibernate.sln"))
			{
				ProjectConfigurations =
				{
					new ProjectConfiguration("NHibernate")
					{
						ScanMethodsBody = true,
						IgnoreExternalReferences = true,
						MethodConversionFunc = symbol =>
						{
							switch (symbol.ContainingType.Name)
							{
								case "IBatcher":
									if (symbol.Name == "ExecuteReader" || symbol.Name == "ExecuteNonQuery")
									{
										return MethodAsyncConversion.ToAsync;
									}
									break;
								case "IAutoFlushEventListener":
								case "IFlushEventListener":
								case "IDeleteEventListener":
								case "ISaveOrUpdateEventListener":
								case "IPostCollectionRecreateEventListener":
								case "IPostCollectionRemoveEventListener":
								case "IPostCollectionUpdateEventListener":
								case "IPostDeleteEventListener":
								case "IPostInsertEventListener":
								case "IPostUpdateEventListener":
								case "IPreCollectionRecreateEventListener":
								case "IPreCollectionRemoveEventListener":
								case "IPreCollectionUpdateEventListener":
								case "IPreDeleteEventListener":
								case "IPreInsertEventListener":
								case "IPreUpdateEventListener":
									return MethodAsyncConversion.ToAsync;
							}
							return MethodAsyncConversion.None;
						},
						FindAsyncCounterpart = findAsyncFn
					},

					new ProjectConfiguration("NHibernate.DomainModel")
					{
						ScanMethodsBody = true,
						IgnoreExternalReferences = true,
						ScanForMissingAsyncMembers = true,
						FindAsyncCounterpart = findAsyncFn
					},

					new ProjectConfiguration("NHibernate.Test")
					{
						ScanForMissingAsyncMembers = true,
						IgnoreExternalReferences = true,
						ScanMethodsBody = true,
						MethodConversionFunc = symbol =>
						{
							// TODO: here should be only Smart
							return symbol.GetAttributes().Any(a => a.AttributeClass.Name == "TestAttribute")
								? MethodAsyncConversion.ToAsync
								: MethodAsyncConversion.Smart;
						},
						CanGenerateMethod = m =>
						{
							return m.ReferenceResults.Any(o => o.Symbol.ContainingAssembly.ToString().StartsWith("NHibernate"));
						},
						CanConvertReferenceFunc = m =>
						{
							return m.ContainingNamespace.ToString() != "System.IO";
						},
						CanScanDocumentFunc = doc =>
						{
							// MathTests is ignored as AsQueryable method is called on a retrieved list from db and the result is used elsewhere in code
							// As we only check if ToList is called on IQueryable we need to ignore it
							//
							// ExpressionSessionLeakTest is also ignored as it looks like that GC.Collect works differently with async.
							// if "await Task.Yield();" is added after DoLinqInSeparateSessionAsync then the test runs successfully (TODO: discover why)
							return !doc.FilePath.EndsWith(@"Linq\MathTests.cs") &&
								   !doc.FilePath.EndsWith(@"Linq\ExpressionSessionLeakTest.cs");
						},
						FindAsyncCounterpart = findAsyncFn,
						TypeTransformationFunc = type =>
						{
							if (type.Name == "LinqReadonlyTestsContext")
							{
								return TypeTransformation.None;
							}
							if (type.GetAttributes().Any(o => o.AttributeClass.Name == "TestFixtureAttribute") || type.Name == "TestCase")
							{
								return TypeTransformation.NewType;
							}
							var baseType = type.BaseType;
							while (baseType != null)
							{
								if (baseType.Name == "TestCase")
								{
									return TypeTransformation.NewType;
								}
								baseType = baseType.BaseType;
							}
							return TypeTransformation.Partial;
						},
						GetAdditionalUsings = doc =>
						{
							return doc.DescendantNodes()
									  .OfType<NamespaceDeclarationSyntax>()
									  .Any(o => o.Name.ToString().StartsWith("NHibernate.Test.Linq"))
								? new[] {"NHibernate.Linq"}
								: null;
						}
					}
				},
				IgnoreProjectNames = new HashSet<string> { "NHibernate.TestDatabaseSetup" },
				TestAttributeNames = new HashSet<string>{ "TestAttribute" },

				IsSymbolValidFunc = (projectInfo, methodSymbol) =>
				{
					return methodSymbol.ContainingType.Name != "NorthwindDbCreator";
				}
			};

			var solutionInfo = new SolutionInfo(solutionConfig);

			var configuration = new TransformerConfiguration
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
					}
				},
				Directive = "NET_4_5",
				PluginFactories = new List<Func<DocumentTransformer, ITransformerPlugin>>
				{
					transformer => new TransactionScopeRewriter(),
					transformer => new XmlEmptyTagTransformerPlugin(transformer)
				}
			};

			AsyncContext.Run(() => solutionInfo.Transform(configuration));
		}
	}
}
