#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1864
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public Task BugAsync()
		{
			try
			{
				Bug();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task FilterOnOffOnAsync()
		{
			try
			{
				FilterOnOffOn();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public Task FilterQueryTwiceAsync()
		{
			try
			{
				FilterQueryTwice();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
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
