#if NET_4_5
using System;
using NHibernate.DomainModel;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LogicalExpressionFixture : BaseExpressionFixture
	{
		[Test]
		public async Task LogicalSqlStringTestAsync()
		{
			ISession session = factory.OpenSession();
			ICriterion orExpression = Expression.Or(Expression.IsNull("Address"), Expression.Between("Count", 5, 10));
			CreateObjects(typeof (Simple), session);
			SqlString sqlString = await (orExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			string expectedSql = "(sql_alias.address is null or sql_alias.count_ between ? and ?)";
			CompareSqlStrings(sqlString, expectedSql, 2);
			session.Close();
		}
	}
}
#endif
