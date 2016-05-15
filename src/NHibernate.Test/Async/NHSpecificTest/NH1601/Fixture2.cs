#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1601
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture2 : BugTestCase
	{
		/// <summary>
		/// Loads the project and when Scenario2 and Scenario3 are set calls Count on the list assigned.
		/// </summary>
		[Test]
		public async Task TestSaveAndLoadWithTwoCountsAsync()
		{
			Project.TestAccessToList = false;
			await (SaveAndLoadProjectAsync());
		}

		/// <summary>
		/// Refreshes the project and when Scenario2 and Scenario3 are set calls Count on the list assigned.
		/// </summary>     
		[Test]
		public async Task TestRefreshWithTwoCountsAsync()
		{
			Project.TestAccessToList = false;
			await (SaveLoadAndRefreshProjectAsync());
		}

		/// <summary>
		/// Loads the project and when Scenario1, Scenario2 and Scenario3 are set calls Count on the list assigned.
		/// </summary>
		[Test]
		public async Task TestTestSaveAndLoadWithThreeCountsAsync()
		{
			Project.TestAccessToList = true;
			await (SaveAndLoadProjectAsync());
		}

		/// <summary>
		/// Refreshes the project and when Scenario1, Scenario2 and Scenario3 are set calls Count on the list assigned.
		/// Throws an exception on calling Count on Scenario1.
		/// </summary>     
		[Test]
		public async Task TestRefreshWithThreeCountsAsync()
		{
			Project.TestAccessToList = true;
			await (SaveLoadAndRefreshProjectAsync());
		}

		/// <summary>
		/// Create and save a Project
		/// </summary>
		public async Task SaveAndLoadProjectAsync()
		{
			await (SaveProjectAsync());
			await (LoadProjectAsync());
		}

		/// <summary>
		/// Create, save and refresh projects
		/// </summary>
		public async Task SaveLoadAndRefreshProjectAsync()
		{
			await (SaveProjectAsync());
			Project project = await (LoadProjectAsync());
			await (RefreshProjectAsync(project));
		}

		public async Task<Project> SaveProjectAsync()
		{
			Project project;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					//Create a project scenario
					project = new Project();
					Scenario scenario1 = new Scenario();
					Scenario scenario2 = new Scenario();
					Scenario scenario3 = new Scenario();
					//Add the scenario to all lists 
					project.ScenarioList1.Add(scenario1);
					project.ScenarioList2.Add(scenario2);
					project.ScenarioList3.Add(scenario3);
					//Set the primary key on the project
					project.Name = "Test";
					//Save the created project
					await (session.SaveAsync(project));
					await (tx.CommitAsync());
				}

			return project;
		}

		public async Task<Project> LoadProjectAsync()
		{
			Project project;
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					//The project is loaded and Scenario1, Scenario2 and Scenario3 properties can be set
					//This will succeed regardless of whether the scenario list is accessed during the set
					project = await (session.GetAsync<Project>("Test"));
					//Commit the transaction and cloase the session
					await (tx.CommitAsync());
				}

			return project;
		}

		public async Task RefreshProjectAsync(Project project)
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					//The project is refreshed and Scenario1, Scenario2 and Scenario3 properties can be set
					//This will succeed when the scenario list is set and accessed during the set but only for
					//Scenario 2 and Scenario3. It will fail if the scenario list is accessed during the set for Scenario1
					await (session.RefreshAsync(project));
				}
		}
	}
}
#endif
