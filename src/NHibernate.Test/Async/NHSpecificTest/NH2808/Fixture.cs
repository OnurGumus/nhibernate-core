#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2808
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task CheckExistanceOfEntityAsync()
		{
			// save an instance of Entity1
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var a = new Entity{Name = "A"};
					await (session.SaveAsync("Entity1", a, 1));
					await (transaction.CommitAsync());
				}

			// check that it is correctly stored in the Entity1 table and does not exist in the Entity2 table.
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = await (session.GetAsync("Entity1", 1));
					Assert.IsNotNull(a);
					a = await (session.GetAsync("Entity2", 1));
					Assert.IsNull(a);
				}
		}

		[Test]
		public async Task UpdateAsync()
		{
			// save an instance of Entity1
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var a = new Entity{Name = "A"};
					await (session.SaveAsync("Entity1", a, 1));
					await (transaction.CommitAsync());
				}

			// load the saved entity, change its name and update.
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var a = (Entity)await (session.GetAsync("Entity1", 1));
					a.Name = "A'";
					await (session.UpdateAsync("Entity1", a, 1));
					await (transaction.CommitAsync());
				}

			// verify
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = (Entity)await (session.GetAsync("Entity1", 1));
					Assert.AreEqual("A'", a.Name);
				}
		}

		[Test]
		public async Task SaveOrUpdateAsync()
		{
			// save an instance of Entity1.
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var a = new Entity{Name = "A"};
					await (session.SaveAsync("Entity1", a, 1));
					await (transaction.CommitAsync());
				}

			// load the entity and adjust its name, create a new entity and save or update both.
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var a = (Entity)await (session.GetAsync("Entity1", 1));
					a.Name = "A'";
					var b = new Entity{Name = "B"};
					await (session.SaveOrUpdateAsync("Entity1", a, 1));
					await (session.SaveOrUpdateAsync("Entity1", b, 2));
					await (transaction.CommitAsync());
				}

			// verify
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = (Entity)await (session.GetAsync("Entity1", 1));
					var b = (Entity)await (session.GetAsync("Entity1", 2));
					Assert.AreEqual("A'", a.Name);
					Assert.AreEqual("B", b.Name);
				}
		}
	}
}
#endif
