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

		/// <summary>
		/// Predicate for method selection
		/// </summary>
		public Func<IMethodSymbol, bool> CanScanMethodFunc { get; set; } = m => false;

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
					},*/
					
					new ProjectConfiguration("NHibernate.DomainModel")
					{
						ScanMethodsBody = true,
						ScanForMissingAsyncMembers = true,
						IgnoreReferencesFromProjects = new HashSet<string> { "NHibernate" }
					},
					new ProjectConfiguration("NHibernate.Test")
					{
						ScanForMissingAsyncMembers = true,
						IgnoreExternalReferences = true,
						//ScanMethodsBody = true,
						//CanScanMethodFunc = symbol => symbol.GetAttributes().Any(a => a.AttributeClass.Name == "TestAttribute")
					}
				},
				IgnoreProjectNames = new HashSet<string> { "NHibernate.TestDatabaseSetup" },
				TestAttributeNames = new HashSet<string>{ "TestAttribute" },
				IsSymbolValidFunc = (projectInfo, methodSymbol) =>
				{
					if(methodSymbol.ContainingAssembly.Name != "NHibernate.Test")
					{
						return true;
					}
					return !new[]
					{
						"TestFixtureSetUp",
						"TestFixtureTearDown",
						"OnSetUp",
						"OnTearDown",
						"AppliesTo",
						"Configure"
					}.Contains(methodSymbol.Name) /*|| methodSymbol.ContainingType.Name == "NorthwindDbCreator"*/;
				}
			};

			var solutionInfo = new SolutionInfo(solutionConfig);
			AsyncContext.Run(() => solutionInfo.Open());
			AsyncContext.Run(() => solutionInfo.Analyze());

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
