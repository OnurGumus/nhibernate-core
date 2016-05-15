#if NET_4_5
using System.Collections.Generic;
using NHibernate.Cfg;
using NUnit.Framework;
using NHibernate.Cfg.Loquacious;
using System.Threading.Tasks;

namespace NHibernate.Test.Hql.Ast
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QuerySubstitutionTest : BaseFixture
	{
		[Test]
		public async Task WhenSubstitutionsConfiguredThenUseItInTranslationAsync()
		{
			const string query = "from SimpleClass s where s.IntValue > pizza";
			var sql = await (GetSqlAsync(query, new Dictionary<string, string>{{"pizza", "1"}}));
			Assert.That(sql, Is.Not.StringContaining("pizza"));
		}

		[Test]
		public async Task WhenExecutedThroughSessionThenUseSubstitutionsAsync()
		{
			const string query = "from SimpleClass s where s.IntValue > pizza";
			using (var s = OpenSession())
			{
				using (SqlLogSpy sqlLogSpy = new SqlLogSpy())
				{
					await (s.CreateQuery(query).ListAsync());
					string sql = sqlLogSpy.Appender.GetEvents()[0].RenderedMessage;
					Assert.That(sql, Is.Not.StringContaining("pizza"));
				}
			}
		}

		[Test]
		public async Task WhenSubstitutionsWithStringConfiguredThenUseItInTranslationAsync()
		{
			const string query = "from SimpleClass s where s.Description > calda";
			var sql = await (GetSqlAsync(query, new Dictionary<string, string>{{"calda", "'bobrock'"}}));
			Assert.That(sql, Is.Not.StringContaining("pizza").And.Contains("'bobrock'"));
		}

		[Test]
		public async Task WhenExecutedThroughSessionThenUseSubstitutionsWithStringAsync()
		{
			const string query = "from SimpleClass s where s.Description > calda";
			using (var s = OpenSession())
			{
				using (SqlLogSpy sqlLogSpy = new SqlLogSpy())
				{
					await (s.CreateQuery(query).ListAsync());
					string sql = sqlLogSpy.Appender.GetEvents()[0].RenderedMessage;
					Assert.That(sql, Is.Not.StringContaining("pizza").And.Contains("'bobrock'"));
				}
			}
		}
	}
}
#endif
