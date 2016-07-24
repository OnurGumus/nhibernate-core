#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1864
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task BugAsync()
		{
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		[Test]
		public async Task FilterOnOffOnAsync()
		{
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s =>
			{
			}

			)));
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		[Test]
		public async Task FilterQueryTwiceAsync()
		{
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
			Assert.DoesNotThrowAsync(async () => await (ExecuteQueryAsync(s => s.EnableFilter("validity").SetParameter("date", DateTime.Now))));
		}

		private async Task ExecuteQueryAsync(Action<ISession> sessionModifier)
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					sessionModifier(session);
					await (session.CreateQuery(@"select cat from Invoice inv, Category cat where cat.ValidUntil = :now and inv.Foo = :foo").SetInt32("foo", 42).SetDateTime("now", DateTime.Now).ListAsync());
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
