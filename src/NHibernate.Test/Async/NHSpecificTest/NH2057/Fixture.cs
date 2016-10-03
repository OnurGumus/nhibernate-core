#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using NHibernate.Impl;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2057
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		[Description("This test fails intermittently on SQL Server ODBC. Not sure why.")]
		public async Task WillCloseWhenUsingDTCAsync()
		{
			SessionImpl s;
			using (var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using (s = (SessionImpl)OpenSession())
				{
					await (s.GetAsync<Person>(1));
				}

				//not closed because the tx is opened yet
				Assert.False(s.IsClosed);
				tx.Complete();
			}

			Assert.That(s.IsClosed, Is.True);
		}
	}
}
#endif
