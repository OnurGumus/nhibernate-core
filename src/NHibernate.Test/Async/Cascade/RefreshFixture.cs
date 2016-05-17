#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Cascade
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class RefreshFixture : TestCase
	{
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
					// behind the session's back, let's modify the statuses
					UpdateStatuses(session);
					// Now lets refresh the persistent batch, and see if the refresh cascaded to the jobs collection elements
					session.Refresh(batch);
					foreach (Job job in batch.Jobs)
					{
						Assert.That(job.Status, Is.EqualTo(1), "Jobs not refreshed!");
					}

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
					session.Refresh(batch);
					Assert.That(batch.Jobs.Count == 1);
					txn.Rollback();
				}
			}
		}
	}
}
#endif
