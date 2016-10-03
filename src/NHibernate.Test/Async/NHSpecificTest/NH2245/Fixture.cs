#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2245
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestDelete_OptimisticLockNoneAsync()
		{
			Guid id;
			// persist a foo instance
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var f = new Foo();
					f.Name = "Henry";
					f.Description = "description";
					await (session.SaveAsync(f));
					await (tx.CommitAsync());
					id = f.Id;
				}
			}

			using (ISession session1 = OpenSession())
				using (ISession session2 = OpenSession())
				{
					// Load the foo from two different sessions. Modify the foo in one session to bump the version in the database, and save. 
					// Then try to delete the foo from the other session. With optimistic lock set to none, this should succeed when the 2245
					// patch is applied to AbstractEntityPersister.cs. Without the patch, NH adds the version to the where clause of the delete
					// statement, and the delete fails.
					var f1 = await (session1.GetAsync<Foo>(id));
					var f2 = await (session2.GetAsync<Foo>(id));
					// Bump version
					using (ITransaction trans1 = session1.BeginTransaction())
					{
						f1.Description = "modified description";
						await (session1.UpdateAsync(f1));
						await (trans1.CommitAsync());
					}

					// Now delete from second session
					using (ITransaction trans2 = session2.BeginTransaction())
					{
						Assert.That(async () => await (session2.DeleteAsync(f2)), Throws.Nothing);
						await (trans2.CommitAsync());
					}
				}

			// Assert that row is really gone
			using (ISession assertSession = OpenSession())
			{
				Assert.IsNull(await (assertSession.GetAsync<Foo>(id)));
			}
		}
	}
}
#endif
