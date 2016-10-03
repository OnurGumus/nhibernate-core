#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH335
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH335";
			}
		}

		private AbcThing[] abcThings;
		private OtherThing[] otherThings;
		private int numAbcThings;
		private int numOtherThings;
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					// Insert a bunch of AbcThings
					numAbcThings = 5;
					this.abcThings = new AbcThing[numAbcThings];
					for (int i = 0; i < numAbcThings; i++)
					{
						AbcThing newAbcThing = new AbcThing();
						// AbcThing.ClassType is automatically generated.
						newAbcThing.ID = Utils.GetRandomID();
						newAbcThing.Name = newAbcThing.ID;
						await (session.SaveAsync(newAbcThing));
					}

					// Insert a bunch of OtherThings
					numOtherThings = 5;
					this.otherThings = new OtherThing[numOtherThings];
					for (int i = 0; i < numOtherThings; i++)
					{
						OtherThing newOtherThing = new OtherThing();
						// OtherThing.ClassType is automatically generated.
						newOtherThing.ID = Utils.GetRandomID();
						newOtherThing.Name = newOtherThing.ID;
						await (session.SaveAsync(newOtherThing));
					}

					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from AbcThing"));
					await (session.DeleteAsync("from OtherThing"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

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
