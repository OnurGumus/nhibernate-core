#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using NHibernate.Hql.Ast.ANTLR;
using System.Threading.Tasks;

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

		/// <summary>
		/// Verifying that a <see langword = "null"/> value passed into SetParameter(name, val) throws
		/// an exception
		/// </summary>
		[Test]
		public void TestNullNamedParameter()
		{
			if (sessions.Settings.QueryTranslatorFactory is ASTQueryTranslatorFactory)
			{
				Assert.Ignore("Not supported; The AST parser can guess the type.");
			}

			ISession s = OpenSession();
			try
			{
				IQuery q = s.CreateQuery("from Simple as s where s.Name=:pName");
				q.SetParameter("pName", null);
				Assert.Fail("should throw if can't guess the type of parameter");
			}
			catch (ArgumentNullException)
			{
			}
			finally
			{
				s.Close();
			}
		}
	}
}
#endif
