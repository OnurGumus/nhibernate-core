#if NET_4_5
using System;
using NHibernate.Driver;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3620
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return (factory.ConnectionProvider.Driver is OracleManagedDataClientDriver);
		}

		protected override Task OnTearDownAsync()
		{
			return CleanupDataAsync();
		}

		[Test]
		public async Task Should_insert_two_blobs_and_a_dateAsync()
		{
			using (ISession s = OpenSession())
			{
				var blob = new Byte[1024 * 24];
				for (int i = 0; i < blob.Length; i++)
				{
					blob[i] = 65;
				}

				using (ITransaction tx = s.BeginTransaction())
				{
					var tb = new TwoBlobs{Blob1 = blob, Blob2 = blob, Id = 1, TheDate = DateTime.Now};
					await (s.SaveAsync(tb));
					await (tx.CommitAsync());
				}
			}
		}

		private async Task CleanupDataAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from TwoBlobs"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
