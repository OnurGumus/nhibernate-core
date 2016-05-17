#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1601
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture1 : BugTestCase
	{
		/// <summary>
		/// Loads the project do not call Count on the list assigned.
		/// </summary>
		[Test]
		public async Task TestSaveAndLoadWithoutCountAsync()
		{
			ProjectWithOneList.TestAccessToList = false;
			await (SaveAndLoadProjectWithOneListAsync());
		}

		/// <summary>
		/// Refreshes the project do not call Count on the list assigned.
		/// </summary>     
		[Test]
		public async Task TestRefreshWithoutCountAsync()
		{
			ProjectWithOneList.TestAccessToList = false;
			await (SaveLoadAndRefreshProjectWithOneListAsync());
		}

		/// <summary>
		/// Loads the project and when Scenario1 is assigned call Count on the list.
		/// </summary>
		[Test]
		public async Task TestSaveAndLoadWithCountAsync()
		{
			ProjectWithOneList.TestAccessToList = true;
			await (SaveAndLoadProjectWithOneListAsync());
		}

		/// <summary>
		/// Refreshes the project and when Scenario1 is assigned call Count on the list.
		/// </summary>     
		[Test]
		public async Task TestRefreshWithCountAsync()
		{
			ProjectWithOneList.TestAccessToList = true;
			await (SaveLoadAndRefreshProjectWithOneListAsync());
		}

		/// <summary>
		/// Create and save a Project
		/// </summary>
		public async Task SaveAndLoadProjectWithOneListAsync()
		{
			await (SaveProjectAsync());
			await (LoadProjectAsync());
		}

		/// <summary>
		/// Create, save and refresh projects
		/// </summary>
		public async Task SaveLoadAndRefreshProjectWithOneListAsync()
		{
			await (SaveProjectAsync());
			ProjectWithOneList project = await (LoadProjectAsync());
			RefreshProject(project);
		}

		public async Task<ProjectWithOneList> SaveProjectAsync()
		{
			ProjectWithOneList project;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					//Create a project scenario
					project = new ProjectWithOneList();
					Scenario scenario = new Scenario();
					//Add the scenario to both lists 
					project.ScenarioList1.Add(scenario);
					//Set the primary key on the project
					project.Name = "Test";
					//Save the created project
					await (session.SaveAsync(project));
					await (tx.CommitAsync());
				}

			return project;
		}

		public async Task<ProjectWithOneList> LoadProjectAsync()
		{
			ProjectWithOneList project;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					//The project is loaded and Scenario1, Scenario2 and Scenario3 properties can be set
					//This will succeed regardless of whether the scenario list is accessed during the set
					project = await (session.GetAsync<ProjectWithOneList>("Test"));
					//Commit the transaction and cloase the session
					await (tx.CommitAsync());
				}

			return project;
		}
	}
}
#endif
