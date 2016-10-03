#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.QueryTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PositionalParametersFixtureAsync : TestCaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new string[]{"Simple.hbm.xml"};
			}
		}

		[Test]
		public async Task TestMissingHQLParametersAsync()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			try
			{
				IQuery q = s.CreateQuery("from s in class Simple where s.Name=? and s.Count=?");
				// Set the first property, but not the second
				q.SetParameter(0, "Fred");
				// Try to execute it
				Assert.ThrowsAsync<QueryException>(async () => await (q.ListAsync()));
			}
			finally
			{
				t.Rollback();
				s.Close();
			}
		}

		[Test]
		public async Task TestMissingHQLParameters2Async()
		{
			ISession s = OpenSession();
			ITransaction t = s.BeginTransaction();
			try
			{
				IQuery q = s.CreateQuery("from s in class Simple where s.Name=? and s.Count=?");
				// Set the second property, but not the first - should give a nice not found at position xxx error
				q.SetParameter(1, "Fred");
				// Try to execute it
				Assert.ThrowsAsync<QueryException>(async () => await (q.ListAsync()));
			}
			finally
			{
				t.Rollback();
				s.Close();
			}
		}
	}
}
#endif
