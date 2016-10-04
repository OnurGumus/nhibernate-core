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

		private Solution OriginalSolution { get; set; }

		internal List<ProjectInfo> ProjectInfos { get; } = new List<ProjectInfo>();

		private async Task Initialize()
		{
			OriginalSolution = Solution = await Workspace.OpenSolutionAsync(Configuration.Path).ConfigureAwait(false);
			//var projectConfigs = Configuration.ProjectConfigurations.ToDictionary(o => o.Name);
			//foreach (var project in OriginalSolution.Projects.Where(o => !Configuration.IgnoreProjectNames.Contains(o.Name)))
			//{
			//	if (!projectConfigs.ContainsKey(project.Name))
			//	{
			//		continue;
			//	}
			//	var config = projectConfigs[project.Name];
			//	var projectInfo = new ProjectInfo(this, project.Id, config);
			//	ProjectInfos.Add(projectInfo);
			//}
		}

		public async Task Generate(WriterConfiguration configuration)
		{
			await Initialize().ConfigureAwait(false);

			var projectConfigs = Configuration.ProjectConfigurations.ToDictionary(o => o.Name);
			foreach (var projectOrig in OriginalSolution.Projects.Where(o => !Configuration.IgnoreProjectNames.Contains(o.Name)))
			{
				if (!projectConfigs.ContainsKey(projectOrig.Name))
				{
					continue;
				}

				var config = projectConfigs[projectOrig.Name];
				var projectInfo = new ProjectInfo(this, projectOrig.Id, config);
				ProjectInfos.Add(projectInfo);
				var modifiedProject = projectInfo.RemoveGeneratedDocuments();
				Solution = modifiedProject.Solution; // update solution as it is immutable
				await projectInfo.Analyze().ConfigureAwait(false);
				projectInfo.PostAnalyze();

				var project = projectInfo.Project;

				foreach (var pair in projectInfo.Where(o => o.Value.Any()))
				{
					var docInfo = pair.Value;
					var writer = new DocumentWriter(docInfo, configuration);
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
					project = project.AddDocument(docInfo.Name, result.NewRootNode.GetText(Encoding.UTF8), folders, $"{writer.DestinationFolder}\\{docInfo.Name}").Project;
					Solution = project.Solution;
				}

				//var origCompilation = await projectOrig.GetCompilationAsync();
				//var origEmit = origCompilation.Emit("output.exe", "output.pdb");
				//var origDiagnostincs = origEmit.Diagnostics;
				//var compilation = await project.GetCompilationAsync();
				//var emit = compilation.Emit("output2.exe", "output2.pdb");
				//var diagnostincs = emit.Diagnostics;
				////var diagnostincs = compilation.GetDeclarationDiagnostics();
				//foreach (var diagnostic in diagnostincs)
				//{
				//}

				ProjectInfos.Clear();
			}
			Workspace.TryApplyChanges(Solution);
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

		//internal SolutionConfiguration SolutionConfiguration { get; set; }

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
		/// Predicate for reference selection
		/// </summary>
		public Func<IMethodSymbol, bool> CanConvertReferenceFunc { get; set; } = m => true;

		/// <summary>
		/// Predicate for reference selection
		/// </summary>
		public Func<MethodInfo, bool> CanGenerateMethod { get; set; } = m => true;

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
		public Func<INamedTypeSymbol, TypeTransformation> TypeTransformationFunc { get; set; } = s => TypeTransformation.Partial; 

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

			AsyncContext.Run(() => solutionInfo.Generate(configuration));
		}
	}
}
