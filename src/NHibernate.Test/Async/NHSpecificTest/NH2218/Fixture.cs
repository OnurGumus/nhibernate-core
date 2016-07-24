#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NHibernate.Test.NHSpecificTest.NH0000;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2218
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					for (int i = 0; i < 4; i++)
					{
						await (session.SaveAsync("Entity1", new Entity{Name = "Mapping1 -" + i}));
					}

					for (int i = 0; i < 3; i++)
					{
						await (session.SaveAsync("Entity2", new Entity{Name = "Mapping2 -" + i}));
					}

					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public void SelectEntitiesByEntityName()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					// verify the instance count for both mappings
					Assert.That(session.Query<Entity>("Entity1").Count(), Is.EqualTo(4));
					Assert.That(session.Query<Entity>("Entity2").Count(), Is.EqualTo(3));
					// verify that all instances are loaded from the right table
					Assert.That(session.Query<Entity>("Entity1").Count(x => x.Name.Contains("Mapping1")), Is.EqualTo(4));
					Assert.That(session.Query<Entity>("Entity2").Count(x => x.Name.Contains("Mapping2")), Is.EqualTo(3));
					// a query for Entity returns instances of both mappings 
					// Remark: session.Query<Entity>().Count() doesn't work because only the count of the first mapping (4) is returned
					Assert.That(session.Query<Entity>().ToList().Count, Is.EqualTo(7));
				}
		}

		[Test]
		public void SelectEntitiesByEntityNameFromStatelessSession()
		{
			using (var session = sessions.OpenStatelessSession())
				using (session.BeginTransaction())
				{
					// verify the instance count for both mappings
					Assert.That(session.Query<Entity>("Entity1").Count(), Is.EqualTo(4));
					Assert.That(session.Query<Entity>("Entity2").Count(), Is.EqualTo(3));
					// verify that all instances are loaded from the right table
					Assert.That(session.Query<Entity>("Entity1").Count(x => x.Name.Contains("Mapping1")), Is.EqualTo(4));
					Assert.That(session.Query<Entity>("Entity2").Count(x => x.Name.Contains("Mapping2")), Is.EqualTo(3));
					// a query for Entity returns instances of both mappings 
					// Remark: session.Query<Entity>().Count() doesn't work because only the count of the first mapping (4) is returned
					Assert.That(session.Query<Entity>().ToList().Count, Is.EqualTo(7));
				}
		}
	}
}
#endif
