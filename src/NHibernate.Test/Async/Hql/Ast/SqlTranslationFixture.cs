#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Hql.Ast
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SqlTranslationFixtureAsync : BaseFixtureAsync
	{
		[Test]
		public async Task ParseFloatConstantAsync()
		{
			const string query = "select 123.5, s from SimpleClass s";
			Assert.That(await (GetSqlAsync(query)), Is.StringStarting("select 123.5"));
		}

		[Test]
		public async Task CaseClauseWithMathAsync()
		{
			const string query = "from SimpleClass s where (case when s.IntValue > 0 then (cast(s.IntValue as long) * :pAValue) else 1 end) > 0";
			Assert.DoesNotThrowAsync(async () => await (GetSqlAsync(query)));
			const string queryWithoutParen = "from SimpleClass s where (case when s.IntValue > 0 then cast(s.IntValue as long) * :pAValue else 1 end) > 0";
			Assert.DoesNotThrowAsync(async () => await (GetSqlAsync(queryWithoutParen)));
		}

		[Test]
		public async Task UnionAsync()
		{
			const string queryForAntlr = "from SimpleClass s where s.id in (select s1.id from SimpleClass s1 union select s2.id from SimpleClass s2)";
			Assert.DoesNotThrowAsync(async () => await (GetSqlAsync(queryForAntlr)));
		}
	}
}
#endif
