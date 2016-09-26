#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using NHibernate.Hql.Ast.ANTLR;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.QueryTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NamedParametersFixtureAsync : TestCaseAsync
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
				IQuery q = s.CreateQuery("from s in class Simple where s.Name=:Name and s.Count=:Count");
				// Just set the Name property not the count
				q.SetAnsiString("Name", "Fred");
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
