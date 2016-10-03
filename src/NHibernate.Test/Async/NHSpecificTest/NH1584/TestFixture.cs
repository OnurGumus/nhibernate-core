#if NET_4_5
/*
    The documentation for NHibernate likes to work with cats / kittens for examples or demonstrations.  
*/
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1584
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TestFixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction trx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Male"));
					await (trx.CommitAsync());
				}
			}
		}

		/// <summary>
		/// Demonstrate that the session is able to load the one-to-one composition between a joined subclass and its related entity. 
		/// </summary>
		[Test]
		public async Task Load_One_To_One_Composition_For_Joined_Subclass_SucceedsAsync()
		{
			var tabby = new Tabby{HasSpots = true, HasStripes = true, HasSwirls = false};
			var newInstance = new Male{Name = "Male", Coat = tabby};
			using (ISession session = OpenSession())
			{
				using (ITransaction trx = session.BeginTransaction())
				{
					await (session.SaveAsync(newInstance));
					await (trx.CommitAsync());
				}
			}

			Assert.AreNotEqual(0, newInstance.Id);
			Assert.AreNotEqual(0, tabby.Id);
			using (ISession session = OpenSession())
			{
				ICriteria criteria = session.CreateCriteria(typeof (Cat));
				var loaded = await (criteria.Add(Restrictions.Eq("Id", newInstance.Id)).UniqueResultAsync<Male>());
				Assert.IsNotNull(loaded.Coat);
			}
		}
	}
}
#endif
