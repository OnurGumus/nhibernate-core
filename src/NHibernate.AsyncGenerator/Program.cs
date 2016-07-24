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
using NHibernate.AsyncGenerator.Extensions;
using Nito.AsyncEx;


[assembly: log4net.Config.XmlConfigurator()]
namespace NHibernate.AsyncGenerator
{
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

		public string Directive { get; set; }

	}

	public class SolutionInfo
	{
		public SolutionInfo(SolutionConfiguration configuration)
		{
			Configuration = configuration;
			Workspace = MSBuildWorkspace.Create();
		}

		internal MSBuildWorkspace Workspace { get; }

		internal readonly SolutionConfiguration Configuration;

		public Solution Solution { get; private set; }

		internal List<ProjectInfo> ProjectInfos { get; } = new List<ProjectInfo>();

		public async Task Open()
		{
			Solution = await Workspace.OpenSolutionAsync(Configuration.Path).ConfigureAwait(false);
			var projectConfigs = Configuration.ProjectConfigurations.ToDictionary(o => o.Name);
			foreach (var project in Solution.Projects.Where(o => !Configuration.IgnoreProjectNames.Contains(o.Name)))
			{
				if (!projectConfigs.ContainsKey(project.Name))
				{
					continue;
				}
				var config = projectConfigs[project.Name];
				config.SolutionConfiguration = Configuration;
				ProjectInfos.Add(new ProjectInfo(project, config) { SolutionInfo = this });
			}
		}

		public async Task Analyze()
		{
			foreach (var projectInfo in ProjectInfos)
			{
				await projectInfo.Analyze().ConfigureAwait(false);
			}
		}

		public void PostAnalyze()
		{
			foreach (var projectInfo in ProjectInfos)
			{
				foreach (var pair in projectInfo.Where(o => o.Value.Any()))
				{
					foreach (var namespaceInfo in pair.Value.Values)
					{
						foreach (var rootTypeInfo in namespaceInfo.Values.Where(o => o.TypeTransformation != TypeTransformation.None))
						{
							foreach (var typeInfo in rootTypeInfo.GetDescendantTypeInfosAndSelf()
																 .Where(o => o.TypeTransformation != TypeTransformation.None))
							{
								foreach (var methodInfo in typeInfo.MethodInfos.Values.Where(o => !o.Ignore))
								{
									methodInfo.PostAnalyze();
								}
							}
						}
					}
				}
			}
		}

		public void Write(WriterConfiguration configuration)
		{
			foreach (var projectInfo in ProjectInfos)
			{
				foreach (var pair in projectInfo.Where(o => o.Value.Any()))
				{
					var writer = new DocumentWriter(pair.Value, configuration);
					writer.Write();
				}
			}
		}
	}

	public class SolutionConfiguration
	{
		public SolutionConfiguration(string path)
		{
			Path = path;
		}

		/// <summary>
		/// Path where the solution is located
		/// </summary>
		public string Path { get; }

		/// <summary>
		/// Configurations for projects inside the solution. If there is no configuration for a project a default one will be used
		/// </summary>
		public List<ProjectConfiguration> ProjectConfigurations { get; } = new List<ProjectConfiguration>();

		/// <summary>
		/// List of project names that will be excluded when analyzing the solution
		/// </summary>
		public HashSet<string> IgnoreProjectNames { get; set; } = new HashSet<string>();

		/// <summary>
		/// Function that will be executed when validating a method symbol. The return value indicates if the symbol is valid or not
		/// </summary>
		public Func<ProjectInfo, IMethodSymbol, bool> IsSymbolValidFunc { get; set; }

		/// <summary>
		/// Names of the attributes used for unit testing. In a unit test we cannot return a Task
		/// </summary>
		public HashSet<string> TestAttributeNames { get; set; } = new HashSet<string>();
	}

	public enum TypeTransformation
	{
		None = 0,
		Partial = 1,
		NewType = 2
	}

	public enum MethodAsyncConversion
	{
		None = 0,
		ToAsync = 1,
		/// <summary>
		/// Will convert to async only if there is invoked at least one method that has an async counterpart
		/// </summary>
		Smart = 3
	}

	public class ProjectConfiguration
	{
		//public static readonly ProjectConfiguration Default = new ProjectConfiguration();

		public ProjectConfiguration(string name)
		{
			Name = name;
		}

		internal SolutionConfiguration SolutionConfiguration { get; set; }

		/// <summary>
		/// Name of the project
		/// </summary>
		public string Name { get; }

		public Func<IMethodSymbol, MethodAsyncConversion> MethodConversionFunc { get; set; } = m => MethodAsyncConversion.None;

		/// <summary>
		/// Predicate for document selection
		/// </summary>
		public Func<Document, bool> CanScanDocumentFunc { get; set; } = m => true;

		/// <summary>
		/// A custom method that will be called when for a method there is not an async counterpart with same parameters
		/// </summary>
		public Func<IMethodSymbol, IMethodSymbol> FindAsyncCounterpart { get; set; } = null;

		/// <summary>
		/// When enabled it will search all invocation expressions inside a method body and tries to find a async counterpart
		/// </summary>
		public bool ScanMethodsBody { get; set; }

		/// <summary>
		/// When enabled it will not analyze references outside project. For ignoring references from specific projects use property IgnoreReferencesFromProjects
		/// </summary>
		public bool IgnoreExternalReferences { get; set; }

		/// <summary>
		/// Ignores all references that are located in the provided project names
		/// </summary>
		public HashSet<string> IgnoreReferencesFromProjects { get; set; } = new HashSet<string>();

		/// <summary>
		/// When enabled it will scan each type in project for non implemented async members that have a sync counterpart
		/// </summary>
		public bool ScanForMissingAsyncMembers { get; set; }

		/// <summary>
		/// Define how types will be converted to async. Default all types will be generated as partial
		/// </summary>
		public Func<INamedTypeSymbol, TypeTransformation> TypeTransformationFunc { get; set; }

		/// <summary>
		/// Name of the folder where all async partial classes will be stored
		/// </summary>
		public string AsyncFolder { get; set; } = "Async";

	}

	class Program
	{
		static void Main(string[] args)
		{
			var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var basePath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\..\"));

			var solutionConfig = new SolutionConfiguration(Path.Combine(basePath, @"NHibernate.sln"))
			{
				ProjectConfigurations =
				{
					/*
					new ProjectConfiguration("NHibernate")
					{
						ScanMethodsBody = true,
						IgnoreExternalReferences = true,
						CanScanMethodFunc = symbol => symbol.GetAttributes().Any(a => a.AttributeClass.Name == "AsyncAttribute"))
					},
					
					new ProjectConfiguration("NHibernate.DomainModel")
					{
						ScanMethodsBody = true,
						ScanForMissingAsyncMembers = true,
						IgnoreReferencesFromProjects = new HashSet<string> { "NHibernate" }
					},*/
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
							//symbol.ContainingType?.ContainingType?.GetAttributes().Any(a => a.AttributeClass.Name == "TestFixtureAttribute") == true
						},
						/*
						CanScanTypeFunc = symbol =>
						{
							if (symbol.Name == "TestCase" || symbol.BaseType?.Name == "TestCase")
							{
								return true;
							}
							return false;
						},*/


						//CanScanDocumentFunc = doc =>
						//{
						//	//return doc.FilePath.EndsWith(@"Interceptor\StatefulInterceptor.cs");
						//	return doc.FilePath.EndsWith(@"NHSpecificTest\NH1882\TestCollectionInitializingDuringFlush.cs"); //||
						//	//doc.FilePath.EndsWith(@"TestCase.cs");
						//},
						FindAsyncCounterpart = symbol =>
						{
							if (symbol.ContainingAssembly.Name != "nunit.framework" && symbol.ContainingType.Name != "Assert")
							{
								return null;
							}
							var delegateNames = new HashSet<string> {"AsyncTestDelegate", "TestDelegate"};
							Func<IParameterSymbol, IParameterSymbol, bool> paramCompareFunc = (p1, p2) =>
							{
								return p1.Type.Equals(p2.Type) || (delegateNames.Contains(p1.Type.Name) && delegateNames.Contains(p2.Type.Name));
							};
							return symbol.ContainingType.GetMembers(symbol.Name + "Async")
										  .OfType<IMethodSymbol>()
										  .Where(o => o.TypeParameters.Length == symbol.TypeParameters.Length)
										  .FirstOrDefault(o => o.HaveSameParameters(symbol, paramCompareFunc));
						},
						TypeTransformationFunc = type =>
						{
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
			//solutionInfo.Open().ConfigureAwait(false).GetAwaiter().GetResult();
			//solutionInfo.Analyze().ConfigureAwait(false).GetAwaiter().GetResult();
			AsyncContext.Run(() => solutionInfo.Open());
			AsyncContext.Run(() => solutionInfo.Analyze());
			solutionInfo.PostAnalyze();

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
				},
				Directive = "NET_4_5"
			};

			solutionInfo.Write(configuration);
		}
	}
}
