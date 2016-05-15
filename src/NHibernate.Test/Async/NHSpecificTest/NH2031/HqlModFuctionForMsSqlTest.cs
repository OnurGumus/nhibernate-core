#if NET_4_5
using NHibernate.Dialect;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2031
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class HqlModFuctionForMsSqlTest : BugTestCase
	{
		[Test]
		public async Task TheModuleOperationShouldAddParenthesisToAvoidWrongSentenceAsync()
		{
			// The expected value should be "(5+1)%(1+1)" instead "5+ 1%1 +1"
			var sqlQuery = await (GetSqlAsync("select mod(5+1,1+1) from MyClass"));
			Assert.That(sqlQuery, Is.StringContaining("(5+1)").And.StringContaining("(1+1)"));
		}

		public async Task<string> GetSqlAsync(string query)
		{
			var qt = new QueryTranslatorImpl(null, new HqlParseEngine(query, false, sessions).Parse(), new CollectionHelper.EmptyMapClass<string, IFilter>(), sessions);
			await (qt.CompileAsync(null, false));
			return qt.SQLString;
		}
	}
}
#endif
