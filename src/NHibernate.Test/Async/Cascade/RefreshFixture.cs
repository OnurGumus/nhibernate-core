#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class RefreshFixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new[]{"Cascade.Job.hbm.xml", "Cascade.JobBatch.hbm.xml"};
			}
		}

		[Test]
		public async Task RefreshCascadeAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					JobBatch batch = new JobBatch(DateTime.Now);
					batch.CreateJob().ProcessingInstructions = "Just do it!";
					batch.CreateJob().ProcessingInstructions = "I know you can do it!";
					// write the stuff to the database; at this stage all job.status values are zero
					await (session.PersistAsync(batch));
					await (session.FlushAsync());
					await (UpdateStatusesAsync(session));
					// Now lets refresh the persistent batch, and see if the refresh cascaded to the jobs collection elements
					await (session.RefreshAsync(batch));
					foreach (Job job in batch.Jobs)
					{
						Assert.That(job.Status, Is.EqualTo(1), "Jobs not refreshed!");
					}

					txn.Rollback();
				}
			}
		}

		private async Task UpdateStatusesAsync(ISession session)
		{
			DbConnection conn = session.Connection;
			DbCommand cmd = conn.CreateCommand();
			cmd.CommandText = "UPDATE T_JOB SET JOB_STATUS = 1";
			cmd.CommandType = CommandType.Text;
			session.Transaction.Enlist(cmd);
			await (cmd.ExecuteNonQueryAsync());
		}

		[Test]
		public async Task RefreshIgnoringTransientAsync()
		{
			// No exception expected
			using (ISession session = OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					var batch = new JobBatch(DateTime.Now);
					await (session.RefreshAsync(batch));
					txn.Rollback();
				}
			}
		}

		[Test]
		public async Task RefreshIgnoringTransientInCollectionAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction txn = session.BeginTransaction())
				{
					var batch = new JobBatch(DateTime.Now);
					batch.CreateJob().ProcessingInstructions = "Just do it!";
					await (session.PersistAsync(batch));
					await (session.FlushAsync());
					batch.CreateJob().ProcessingInstructions = "I know you can do it!";
					await (session.RefreshAsync(batch));
					Assert.That(batch.Jobs.Count == 1);
					txn.Rollback();
				}
			}
		}

		[Test]
		public async Task RefreshNotIgnoringTransientByUnsavedValueAsync()
		{
			ISession session = OpenSession();
			ITransaction txn = session.BeginTransaction();
			var batch = new JobBatch{BatchDate = DateTime.Now, Id = 1};
			try
			{
				await (session.RefreshAsync(batch));
			}
			catch (UnresolvableObjectException)
			{
				// as expected
				txn.Rollback();
				session.Close();
			}
		}
	}
}
#endif
