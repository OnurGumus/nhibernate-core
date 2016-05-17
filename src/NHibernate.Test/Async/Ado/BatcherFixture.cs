#if NET_4_5
using System.Collections;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Ado
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BatcherFixture : TestCase
	{
		[Test]
		[Description("The batcher should run all INSERT queries in only one roundtrip.")]
		public async Task OneRoundTripInsertsAsync()
		{
			sessions.Statistics.Clear();
			await (FillDbAsync());
			Assert.That(sessions.Statistics.PrepareStatementCount, Is.EqualTo(1));
			await (CleanupAsync());
		}

		private async Task CleanupAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (s.BeginTransaction())
				{
					s.CreateQuery("delete from VerySimple").ExecuteUpdate();
					s.CreateQuery("delete from AlmostSimple").ExecuteUpdate();
					await (s.Transaction.CommitAsync());
				}
		}

		private async Task FillDbAsync()
		{
			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new VerySimple{Id = 1, Name = "Fabio", Weight = 119.5}));
					await (s.SaveAsync(new VerySimple{Id = 2, Name = "Fiamma", Weight = 9.8}));
					await (tx.CommitAsync());
				}
		}

		[Test]
		[Description("The batcher should run all UPDATE queries in only one roundtrip.")]
		public async Task OneRoundTripUpdateAsync()
		{
			await (FillDbAsync());
			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var vs1 = await (s.GetAsync<VerySimple>(1));
					var vs2 = await (s.GetAsync<VerySimple>(2));
					vs1.Weight -= 10;
					vs2.Weight -= 1;
					sessions.Statistics.Clear();
					await (s.UpdateAsync(vs1));
					await (s.UpdateAsync(vs2));
					await (tx.CommitAsync());
				}

			Assert.That(sessions.Statistics.PrepareStatementCount, Is.EqualTo(1));
			await (CleanupAsync());
		}

		[Test, Ignore("Not fixed yet.")]
		[Description("SqlClient: The batcher should run all different INSERT queries in only one roundtrip.")]
		public async Task SqlClientOneRoundTripForUpdateAndInsertAsync()
		{
			if (sessions.Settings.BatcherFactory is SqlClientBatchingBatcherFactory == false)
				Assert.Ignore("This test is for SqlClientBatchingBatcher only");
			await (FillDbAsync());
			using (var sqlLog = new SqlLogSpy())
				using (ISession s = sessions.OpenSession())
					using (ITransaction tx = s.BeginTransaction())
					{
						await (s.SaveAsync(new VerySimple{Name = "test441", Weight = 894}));
						await (s.SaveAsync(new AlmostSimple{Name = "test441", Weight = 894}));
						await (tx.CommitAsync());
						var log = sqlLog.GetWholeLog();
						//log should only contain NHibernate.SQL once, because that means 
						//that we ony generated a single batch (NHibernate.SQL log will output
						//once per batch)
						Assert.AreEqual(0, log.IndexOf("NHibernate.SQL"), "log should start with NHibernate.SQL");
						Assert.AreEqual(-1, log.IndexOf("NHibernate.SQL", "NHibernate.SQL".Length), "NHibernate.SQL should only appear once in the log");
					}

			await (CleanupAsync());
		}

		[Test]
		[Description("SqlClient: The batcher log output should be formatted")]
		public async Task BatchedoutputShouldBeFormattedAsync()
		{
			if (sessions.Settings.BatcherFactory is SqlClientBatchingBatcherFactory == false)
				Assert.Ignore("This test is for SqlClientBatchingBatcher only");
			using (var sqlLog = new SqlLogSpy())
			{
				await (FillDbAsync());
				var log = sqlLog.GetWholeLog();
				Assert.IsTrue(log.Contains("INSERT \n    INTO"));
			}

			await (CleanupAsync());
		}

		[Test]
		[Description("The batcher should run all DELETE queries in only one roundtrip.")]
		public async Task OneRoundTripDeleteAsync()
		{
			await (FillDbAsync());
			using (ISession s = sessions.OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var vs1 = await (s.GetAsync<VerySimple>(1));
					var vs2 = await (s.GetAsync<VerySimple>(2));
					sessions.Statistics.Clear();
					await (s.DeleteAsync(vs1));
					await (s.DeleteAsync(vs2));
					await (tx.CommitAsync());
				}

			Assert.That(sessions.Statistics.PrepareStatementCount, Is.EqualTo(1));
			await (CleanupAsync());
		}

		[Test]
		[Description(@"Activating the SQL and turning off the batcher's log the log stream:
-should not contains adding to batch
-should contain batch command
-the batcher should work.")]
		public async Task SqlLogAsync()
		{
			using (new LogSpy(typeof (AbstractBatcher), true))
			{
				using (var sl = new SqlLogSpy())
				{
					sessions.Statistics.Clear();
					await (FillDbAsync());
					string logs = sl.GetWholeLog();
					Assert.That(logs, Is.Not.StringContaining("Adding to batch").IgnoreCase);
					Assert.That(logs, Is.StringContaining("Batch command").IgnoreCase);
					Assert.That(logs, Is.StringContaining("INSERT").IgnoreCase);
				}
			}

			Assert.That(sessions.Statistics.PrepareStatementCount, Is.EqualTo(1));
			await (CleanupAsync());
		}

		[Test]
		[Description(@"Activating the AbstractBatcher's log the log stream:
-should not contains batch info 
-should contain SQL log info only regarding batcher (SQL log should not be duplicated)
-the batcher should work.")]
		public async Task AbstractBatcherLogAsync()
		{
			using (new LogSpy(typeof (AbstractBatcher)))
			{
				using (var sl = new SqlLogSpy())
				{
					sessions.Statistics.Clear();
					await (FillDbAsync());
					string logs = sl.GetWholeLog();
					Assert.That(logs, Is.StringContaining("batch").IgnoreCase);
					foreach (var loggingEvent in sl.Appender.GetEvents())
					{
						string message = loggingEvent.RenderedMessage;
						if (message.ToLowerInvariant().Contains("insert"))
						{
							Assert.That(message, Is.StringContaining("batch").IgnoreCase);
						}
					}
				}
			}

			Assert.That(sessions.Statistics.PrepareStatementCount, Is.EqualTo(1));
			await (CleanupAsync());
		}

		[Test]
		public async Task SqlLogShouldGetBatchCommandNotificationAsync()
		{
			using (new LogSpy(typeof (AbstractBatcher)))
			{
				using (var sl = new SqlLogSpy())
				{
					sessions.Statistics.Clear();
					await (FillDbAsync());
					string logs = sl.GetWholeLog();
					Assert.That(logs, Is.StringContaining("Batch commands:").IgnoreCase);
				}
			}

			Assert.That(sessions.Statistics.PrepareStatementCount, Is.EqualTo(1));
			await (CleanupAsync());
		}

		[Test]
		[Description(@"Activating the AbstractBatcher's log the log stream:
-should contain well formatted SQL log info")]
		public async Task AbstractBatcherLogFormattedSqlAsync()
		{
			using (new LogSpy(typeof (AbstractBatcher)))
			{
				using (var sl = new SqlLogSpy())
				{
					sessions.Statistics.Clear();
					await (FillDbAsync());
					foreach (var loggingEvent in sl.Appender.GetEvents())
					{
						string message = loggingEvent.RenderedMessage;
						if (message.StartsWith("Adding"))
						{
							// should be the line with the formatted SQL
							var strings = message.Split(System.Environment.NewLine.ToCharArray());
							foreach (var sqlLine in strings)
							{
								if (sqlLine.Contains("p0"))
								{
									Assert.That(sqlLine, Is.StringContaining("p1"));
									Assert.That(sqlLine, Is.StringContaining("p2"));
								}
							}
						}
					}
				}
			}

			Assert.That(sessions.Statistics.PrepareStatementCount, Is.EqualTo(1));
			await (CleanupAsync());
		}
	}
}
#endif
