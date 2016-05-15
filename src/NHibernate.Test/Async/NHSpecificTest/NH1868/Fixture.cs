#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

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
					await (session.RefreshAsync(cat));
					await (session.RefreshAsync(package));
					await ((await ((await (session.CreateQuery(@"
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
                    ").SetEntityAsync("cat", cat))).SetEntityAsync("package", package))).SetDateTime("now", DateTime.Now).UniqueResultAsync<Invoice>());
					await (tx.CommitAsync());
				}
			}
		}

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

		[Test]
		public Task FilterQuery3Async()
		{
			try
			{
				FilterQuery3();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
