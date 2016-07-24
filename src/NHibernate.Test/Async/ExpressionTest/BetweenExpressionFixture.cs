#if NET_4_5
using NHibernate.DomainModel;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BetweenExpressionFixtureAsync : BaseExpressionFixture
	{
		[Test]
		public async Task BetweenSqlStringTestAsync()
		{
			ISession session = factory.OpenSession();
			CreateObjects(typeof (Simple), session);
			ICriterion betweenExpression = Expression.Between("Count", 5, 10);
			SqlString sqlString = await (betweenExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			string expectedSql = "sql_alias.count_ between ? and ?";
			CompareSqlStrings(sqlString, expectedSql, 2);
			session.Close();
		}
	}
}
#endif
