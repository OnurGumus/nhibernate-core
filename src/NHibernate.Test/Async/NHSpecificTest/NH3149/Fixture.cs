#if NET_4_5
using System.Diagnostics;
using System.Threading;
using NHibernate.Dialect;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3149
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
