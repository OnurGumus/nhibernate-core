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

			Func<IMethodSymbol, IMethodSymbol> findAsyncFn = symbol =>
			{
				var ns = symbol.ContainingNamespace?.ToString() ?? "";
				if (ns.StartsWith("System") && !ns.StartsWith("System.Data"))
				{
					// TODO: handle Linq
					return null;
				}

				Func<IParameterSymbol, IParameterSymbol, bool> paramCompareFunc = null;
				if (symbol.ContainingAssembly.Name == "nunit.framework" && symbol.ContainingType.Name == "Assert")
				{
					var delegateNames = new HashSet<string> { "AsyncTestDelegate", "TestDelegate" };
					paramCompareFunc = (p1, p2) => p1.Type.Equals(p2.Type) ||
												   (delegateNames.Contains(p1.Type.Name) && delegateNames.Contains(p2.Type.Name));
				}
				return symbol.ContainingType.EnumerateBaseTypesAndSelf()
					.SelectMany(o => o.GetMembers(symbol.Name + "Async"))
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
							if (symbol.GetAttributes().Any(a => a.AttributeClass.Name == "AsyncAttribute"))
							{
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
							if (symbol.GetAttributes().Any(a => a.AttributeClass.Name == "TestAttribute"))
							{
								return MethodAsyncConversion.ToAsync;
							}
							return MethodAsyncConversion.Smart;
						},
						CanGenerateMethod = m =>
						{
							return
								m.ReferenceResults.Any(o => o.Symbol.ContainingAssembly.ToString().StartsWith("NHibernate"));
						},
						CanConvertReferenceFunc = m =>
						{
							return m.ContainingNamespace.ToString() != "System.IO";
						},
						//CanScanDocumentFunc = doc =>
						//{
						//	//return !doc.FilePath.Contains("NHSpecificTest") && (
						//	//	doc.FilePath.EndsWith(@"MultiPathCircleCascadeTest.cs") ||
						//	//	doc.FilePath.EndsWith(@"ComponentTest.cs") ||
						//	//	doc.FilePath.EndsWith(@"\TestCase.cs")
						//	//	);
							
						//	return doc.FilePath.EndsWith(@"Insertordering\InsertOrderingFixture.cs") ||
						//	//doc.FilePath.EndsWith(@"NH2583\AbstractMassTestingFixture.cs") ||
						//	//doc.FilePath.EndsWith(@"NHSpecificTest\BugTestCase.cs") ||
						//	//doc.FilePath.EndsWith(@"NH2583\Domain.cs") ||
						//	doc.FilePath.EndsWith(@"\TestCase.cs");
							
						//	//return
						//	//doc.FilePath.EndsWith(@"NHSpecificTest\BasicClassFixture.cs") ||
						//	//doc.FilePath.EndsWith(@"\ObjectAssertion.cs") ||
						//	//doc.FilePath.EndsWith(@"\TestCase.cs");
						//},
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
						}
					}
				},
				IgnoreProjectNames = new HashSet<string> { "NHibernate.TestDatabaseSetup" },
				TestAttributeNames = new HashSet<string>{ "TestAttribute" },

				IsSymbolValidFunc = (projectInfo, methodSymbol) =>
				{
					if (methodSymbol.ContainingType.Name == "NorthwindDbCreator")
					{
						return false;
					}
					return true;
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
					},
					AttributeName = "Async"
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
