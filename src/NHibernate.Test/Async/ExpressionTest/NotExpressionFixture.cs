#if NET_4_5
using NHibernate.Dialect;
using NHibernate.DomainModel;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NotExpressionFixture : BaseExpressionFixture
	{
		[Test]
		public async Task NotSqlStringTestAsync()
		{
			ISession session = factory.OpenSession();
			ICriterion notExpression = Expression.Not(Expression.Eq("Address", "12 Adress"));
			CreateObjects(typeof (Simple), session);
			SqlString sqlString = await (notExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			string expectedSql = "not (sql_alias.address = ?)";
			CompareSqlStrings(sqlString, expectedSql, 1);
			session.Close();
		}
	}
}
#endif
