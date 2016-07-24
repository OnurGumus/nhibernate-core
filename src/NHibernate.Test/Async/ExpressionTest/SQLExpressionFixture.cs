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
	public partial class SQLExpressionFixtureAsync : BaseExpressionFixture
	{
		[Test]
		public async Task StraightSqlTestAsync()
		{
			using (ISession session = factory.OpenSession())
			{
				ICriterion sqlExpression = Expression.Sql("{alias}.address is not null");
				CreateObjects(typeof (Simple), session);
				SqlString sqlString = await (sqlExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
				string expectedSql = "sql_alias.address is not null";
				CompareSqlStrings(sqlString, expectedSql);
			}
		}

		[Test]
		public async Task NoParamsSqlStringTestAsync()
		{
			using (ISession session = factory.OpenSession())
			{
				ICriterion sqlExpression = Expression.Sql(new SqlString("{alias}.address is not null"));
				CreateObjects(typeof (Simple), session);
				SqlString sqlString = await (sqlExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
				string expectedSql = "sql_alias.address is not null";
				CompareSqlStrings(sqlString, expectedSql);
			}
		}

		[Test]
		public async Task WithParameterTestAsync()
		{
			using (ISession session = factory.OpenSession())
			{
				SqlStringBuilder builder = new SqlStringBuilder();
				string expectedSql = "sql_alias.address = ?";
				builder.Add("{alias}.address = ");
				builder.AddParameter();
				ICriterion sqlExpression = Expression.Sql(builder.ToSqlString(), "some address", NHibernateUtil.String);
				CreateObjects(typeof (Simple), session);
				SqlString sqlString = await (sqlExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
				CompareSqlStrings(sqlString, expectedSql, 1);
			}
		}
	}
}
#endif
