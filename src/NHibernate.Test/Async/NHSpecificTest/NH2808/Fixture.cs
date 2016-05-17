#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2808
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CheckExistanceOfEntityAsync()
		{
			// save an instance of Entity1
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var a = new Entity{Name = "A"};
					session.Save("Entity1", a, 1);
					await (transaction.CommitAsync());
				}

			// check that it is correctly stored in the Entity1 table and does not exist in the Entity2 table.
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = session.Get("Entity1", 1);
					Assert.IsNotNull(a);
					a = session.Get("Entity2", 1);
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
					session.Save("Entity1", a, 1);
					await (transaction.CommitAsync());
				}

			// load the saved entity, change its name and update.
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var a = (Entity)session.Get("Entity1", 1);
					a.Name = "A'";
					session.Update("Entity1", a, 1);
					await (transaction.CommitAsync());
				}

			// verify
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = (Entity)session.Get("Entity1", 1);
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
					session.Save("Entity1", a, 1);
					await (transaction.CommitAsync());
				}

			// load the entity and adjust its name, create a new entity and save or update both.
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var a = (Entity)session.Get("Entity1", 1);
					a.Name = "A'";
					var b = new Entity{Name = "B"};
					session.SaveOrUpdate("Entity1", a, 1);
					session.SaveOrUpdate("Entity1", b, 2);
					await (transaction.CommitAsync());
				}

			// verify
			using (ISession session = OpenSession())
				using (session.BeginTransaction())
				{
					var a = (Entity)session.Get("Entity1", 1);
					var b = (Entity)session.Get("Entity1", 2);
					Assert.AreEqual("A'", a.Name);
					Assert.AreEqual("B", b.Name);
				}
		}
	}
}
#endif
