#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1864
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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

		private async Task ExecuteQueryAsync(Action<ISession> sessionModifier)
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					sessionModifier(session);
					session.CreateQuery(@"select cat from Invoice inv, Category cat where cat.ValidUntil = :now and inv.Foo = :foo").SetInt32("foo", 42).SetDateTime("now", DateTime.Now).List();
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
