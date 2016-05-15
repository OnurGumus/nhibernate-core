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
	public partial class NullExpressionFixture : BaseExpressionFixture
	{
		[Test]
		public async Task NullSqlStringTestAsync()
		{
			ISession session = factory.OpenSession();
			ICriterion expression = Expression.IsNull("Address");
			CreateObjects(typeof (Simple), session);
			SqlString sqlString = await (expression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			string expectedSql = "sql_alias.address is null";
			CompareSqlStrings(sqlString, expectedSql, 0);
			session.Close();
		}
	}
}
#endif
