using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
		public SolutionInfo()
		{
			Workspace = MSBuildWorkspace.Create();
		}

		internal MSBuildWorkspace Workspace { get; }

		public Solution Solution { get; private set; }

		internal List<ProjectInfo> ProjectInfos { get; } = new List<ProjectInfo>();

		public async Task Open(string path)
		{
			Solution = await Workspace.OpenSolutionAsync(path).ConfigureAwait(false);
			foreach (var project in Solution.Projects)
			{
				ProjectInfos.Add(new ProjectInfo(project));
			}
		}

		public async Task AddProject(string path)
		{
			var project = await Workspace.OpenProjectAsync(path).ConfigureAwait(false);
			ProjectInfos.Add(new ProjectInfo(project));
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
				foreach (var pair in projectInfo.Where(o => o.Value.NamespaceInfos.Any()))
				{
					var writer = new DocumentWriter(pair.Value, configuration);
					writer.Write();
				}
			}
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			var currentPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var basePath = Path.GetFullPath(Path.Combine(currentPath, @"..\..\..\"));

			var solutionInfo = new SolutionInfo();
			AsyncContext.Run(() => solutionInfo.Open(Path.Combine(basePath, @"NHibernate.sln")));
			//AsyncContext.Run(() => solutionInfo.AddProject(Path.Combine(basePath, @"NHibernate\NHibernate.csproj")));
			//AsyncContext.Run(() => solutionInfo.AddProject(Path.Combine(basePath, @"NHibernate.DomainModel\NHibernate.DomainModel.csproj")));
			//AsyncContext.Run(() => solutionInfo.AddProject(Path.Combine(basePath, @"NHibernate.Test\NHibernate.Test.csproj")));
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
