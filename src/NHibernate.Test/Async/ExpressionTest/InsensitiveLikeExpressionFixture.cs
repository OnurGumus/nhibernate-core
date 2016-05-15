#if NET_4_5
using System;
using NHibernate.Dialect;
using NHibernate.DomainModel;
using NHibernate.Engine;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InsensitiveLikeExpressionFixture : BaseExpressionFixture
	{
		[Test]
		public async Task InsentitiveLikeSqlStringTestAsync()
		{
			ISession session = factory.OpenSession();
			ICriterion expression = Expression.InsensitiveLike("Address", "12 Adress");
			CreateObjects(typeof (Simple), session);
			SqlString sqlString = await (expression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			string expectedSql = "lower(sql_alias.address) like ?";
			if ((factory as ISessionFactoryImplementor).Dialect is PostgreSQLDialect)
			{
				expectedSql = "sql_alias.address ilike ?";
			}

			CompareSqlStrings(sqlString, expectedSql, 1);
			session.Close();
		}
	}
}
#endif
