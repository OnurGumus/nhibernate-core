using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;

namespace NHibernate.AsyncGenerator
{
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

		private Solution OriginalSolution { get; set; }

		internal List<ProjectInfo> ProjectInfos { get; } = new List<ProjectInfo>();

		private async Task Initialize()
		{
			OriginalSolution = Solution = await Workspace.OpenSolutionAsync(Configuration.Path).ConfigureAwait(false);
			var projectConfigs = Configuration.ProjectConfigurations.ToDictionary(o => o.Name);
			foreach (var project in OriginalSolution.Projects.Where(o => !Configuration.IgnoreProjectNames.Contains(o.Name)))
			{
				if (!projectConfigs.ContainsKey(project.Name))
				{
					continue;
				}
				var config = projectConfigs[project.Name];
				var projectInfo = new ProjectInfo(this, project.Id, config);
				var modifiedProject = projectInfo.RemoveGeneratedDocuments();
				Solution = modifiedProject.Solution; // update solution as it is immutable
				ProjectInfos.Add(projectInfo);
			}
		}

		/// <summary>
		/// Transforms all sync methods that can be async to async ones
		/// </summary>
		/// <param name="configuration"></param>
		/// <returns></returns>
		public async Task Transform(TransformerConfiguration configuration)
		{
			await Initialize().ConfigureAwait(false);

			foreach (var projectInfo in ProjectInfos)
			{
				await projectInfo.Analyze().ConfigureAwait(false);
				projectInfo.PostAnalyze();

				var project = projectInfo.Project;

				foreach (var pair in projectInfo.Where(o => o.Value.Any()))
				{
					var docInfo = pair.Value;
					var writer = new DocumentTransformer(docInfo, configuration);
					var result = writer.Transform();
					if (result == null)
					{
						continue;
					}
					if (result.OriginalRootNode != null)
					{
						project = project.GetDocument(docInfo.Document.Id).WithSyntaxRoot(result.OriginalRootNode).Project;
					}
					var folders = new List<string> { projectInfo.Configuration.AsyncFolder }.Union(docInfo.Folders);
					project = project.AddDocument(docInfo.Name, result.TransformedNode.GetText(Encoding.UTF8), folders, $"{writer.DestinationFolder}\\{docInfo.Name}").Project;
					Solution = project.Solution;
				}

				//var origCompilation = await projectOrig.GetCompilationAsync();
				//var origEmit = origCompilation.Emit("output.exe", "output.pdb");
				//var origDiagnostincs = origEmit.Diagnostics;
				var compilation = await project.GetCompilationAsync();
				var emit = compilation.Emit("output2.exe", "output2.pdb");
				if (!emit.Success)
				{
					var messages = string.Join(
						Environment.NewLine,
						emit.Diagnostics.Where(o => o.Severity == DiagnosticSeverity.Error).Select(o => o.GetMessage()));
					throw new InvalidOperationException(
						$"Generation for Project {project.Name} failed to generate a valid code. Errors:{Environment.NewLine}{messages}");
				}
				var diagnostincs = emit.Diagnostics.Where(o => o.Severity != DiagnosticSeverity.Hidden).ToList();
				foreach (var diagnostic in diagnostincs)
				{
				}
			}
			Workspace.TryApplyChanges(Solution);
		}
	}
}
