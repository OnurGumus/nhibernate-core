#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using NHibernate.Dialect;
using NHibernate.Impl;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2077
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return dialect is MsSql2000Dialect;
		}

		[Test]
		public async Task CanExecuteMultipleQueriesUsingNativeSQLAsync()
		{
			using (var s = OpenSession())
			{
				await (s.CreateSQLQuery(@"
DELETE FROM Person WHERE Id = :userId; 
UPDATE Person SET Id = :deletedUserId WHERE Id = :userId; 
DELETE FROM Person WHERE Id = :userId; 
").SetParameter("userId", 1).SetParameter("deletedUserId", 1).ExecuteUpdateAsync());
			}
		}
	}
}
#endif
