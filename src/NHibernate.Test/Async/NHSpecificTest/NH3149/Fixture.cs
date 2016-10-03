#if NET_4_5
using System.Diagnostics;
using System.Threading;
using NHibernate.Dialect;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3149
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var entity = new NH3149Entity();
					await (session.SaveAsync(entity));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from NH3149Entity"));
					await (tx.CommitAsync());
				}
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect;
		}

		[Test]
		public async Task ShouldNotWaitForLockAsync()
		{
			using (var session1 = OpenSession())
			{
				var thread = new Thread(() =>
				{
					session1.BeginTransaction();
					var entity = session1.CreateCriteria<NH3149Entity>().SetLockMode(LockMode.UpgradeNoWait).AddOrder(Order.Desc("Id")).SetMaxResults(1).UniqueResult();
				}

				);
				thread.Start();
				Thread.Sleep(1000);
				var watch = new Stopwatch();
				watch.Start();
				try
				{
					using (var session = OpenSession())
						using (var tx = session.BeginTransaction())
						{
							var entity = await (session.CreateCriteria<NH3149Entity>().SetTimeout(3).SetLockMode(LockMode.UpgradeNoWait).AddOrder(Order.Desc("Id")).SetMaxResults(1).UniqueResultAsync());
						}
				}
				catch
				{
				// Ctach lack time out error
				}

				watch.Stop();
				thread.Join();
				Assert.That(watch.ElapsedMilliseconds, Is.LessThan(3000));
			}
		}
	}
}
#endif
