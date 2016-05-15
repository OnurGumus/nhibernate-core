#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH335
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task SelectSuperclassAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					IQuery query = session.CreateQuery("from Thing");
					IList list = await (query.ListAsync());
					Assert.AreEqual(numAbcThings + numOtherThings, list.Count, String.Format("There should be {0} Things.", numAbcThings + numOtherThings));
					foreach (object thing in list)
					{
						Assert.IsTrue(thing is Thing);
					}

					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task SelectSubclassAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					IQuery query = session.CreateQuery("from AbcThing");
					IList list = await (query.ListAsync());
					Assert.AreEqual(numAbcThings, list.Count, String.Format("There should be {0} AbcThings.", numAbcThings));
					foreach (object thing in list)
					{
						Assert.IsTrue(thing is AbcThing);
					}

					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					IQuery query = session.CreateQuery("from OtherThing");
					IList list = await (query.ListAsync());
					Assert.AreEqual(numAbcThings, list.Count, String.Format("There should be {0} OtherThings.", numAbcThings));
					foreach (object thing in list)
					{
						Assert.IsTrue(thing is OtherThing);
					}

					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task DeleteAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from AbcThing"));
					await (tx.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					IQuery abcThingQuery = session.CreateQuery("from AbcThing");
					IList abcThings = await (abcThingQuery.ListAsync());
					Assert.AreEqual(0, abcThings.Count, "All AbcThings should have been deleted.");
					IQuery otherThingQuery = session.CreateQuery("from OtherThing");
					IList otherThings = await (otherThingQuery.ListAsync());
					Assert.AreEqual(numOtherThings, otherThings.Count, "No OtherThings should have been deleted.");
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
