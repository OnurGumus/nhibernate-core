#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Event;
using NHibernate.Id;
using NHibernate.Transaction;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1326
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1326";
			}
		}

		[Test]
		public async Task ShouldThrowIfCallingDisconnectInsideTransactionAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Assert.Throws<InvalidOperationException>(() => s.Disconnect(), "Disconnect cannot be called while a transaction is in progress.");
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
