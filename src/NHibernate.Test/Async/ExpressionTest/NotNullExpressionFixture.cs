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
	public partial class NotNullExpressionFixtureAsync : BaseExpressionFixture
	{
		[Test]
		public async Task NotNullSqlStringTestAsync()
		{
			ISession session = factory.OpenSession();
			ICriterion notNullExpression = Expression.IsNotNull("Address");
			CreateObjects(typeof (Simple), session);
			SqlString sqlString = await (notNullExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			string expectedSql = "sql_alias.address is not null";
			CompareSqlStrings(sqlString, expectedSql, 0);
			session.Close();
		}
	}
}
#endif
