#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1850
{
	using System;
	using AdoNet;
	using Environment = NHibernate.Cfg.Environment;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanGetQueryDurationForBatchAsync()
		{
			using (LogSpy spy = new LogSpy(typeof (AbstractBatcher)))
				using (ISession session = OpenSession())
					using (ITransaction tx = session.BeginTransaction())
					{
						for (int i = 0; i < 3; i++)
						{
							var customer = new Customer{Name = "foo"};
							await (session.SaveAsync(customer));
							await (session.DeleteAsync(customer));
						}

						await (session.FlushAsync());
						var wholeLog = spy.GetWholeLog();
						var lines = wholeLog.Split(new[]{System.Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
						int batches = 0;
						foreach (var line in lines)
						{
							if (line.Contains("ExecuteBatch for 1 statements took "))
								batches += 1;
						}

						Assert.AreEqual(3, batches);
						tx.Rollback();
					}
		}
	}
}
#endif
