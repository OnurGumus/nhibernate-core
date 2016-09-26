#if NET_4_5
using log4net.Core;
using NHibernate.AdoNet;
using NHibernate.Cfg;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1144
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private Configuration configuration;
		public override string BugNumber
		{
			get
			{
				return "NH1144";
			}
		}

		protected override void Configure(Configuration configuration)
		{
			this.configuration = configuration;
			this.configuration.Properties[Environment.BatchSize] = "10";
		}

		[Test]
		public async Task CanSaveInSingleBatchAsync()
		{
			if (configuration.Properties[Environment.ConnectionDriver].Contains(typeof (OracleDataClientDriver).Name) == false)
			{
				Assert.Ignore("Only applicable for Oracle Data Client driver");
			}

			MainClass[] mc = new MainClass[]{new MainClass("d0"), new MainClass("d0"), new MainClass("d1"), new MainClass("d1"), new MainClass("d1")};
			bool executedBatch = false;
			using (LogSpy spy = new LogSpy(typeof (AbstractBatcher)))
			{
				using (ISession s = OpenSession())
				{
					using (ITransaction tx = s.BeginTransaction())
					{
						foreach (MainClass mainClass in mc)
						{
							await (s.SaveAsync(mainClass));
						}

						await (tx.CommitAsync());
						foreach (LoggingEvent loggingEvent in spy.Appender.GetEvents())
						{
							if ("Executing batch".Equals(loggingEvent.MessageObject))
							{
								executedBatch = true;
								break;
							}
						}
					}
				}
			}

			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from MainClass"));
					await (tx.CommitAsync());
				}
			}

			Assert.IsTrue(executedBatch);
		}
	}
}
#endif
