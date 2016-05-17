#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1868
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		public async Task ExecuteQueryAsync(Action<ISession> sessionModifier)
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					sessionModifier(session);
					session.Refresh(cat);
					session.Refresh(package);
					session.CreateQuery(@"
                    select 
                        inv
                    from 
                        Invoice inv
                        , Package p
                    where
                        p = :package
                        and inv.Category = :cat
                        and inv.ValidUntil > :now
                        and inv.Package = :package 
                    ").SetEntity("cat", cat).SetEntity("package", package).SetDateTime("now", DateTime.Now).UniqueResult<Invoice>();
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task BugAsync()
		{
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		[Test]
		public async Task FilterOnOffOnAsync()
		{
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s =>
			{
			}

			)));
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		[Test]
		public async Task FilterQueryTwiceAsync()
		{
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		[Test]
		public async Task FilterQuery3Async()
		{
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrow(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}
	}
}
#endif
