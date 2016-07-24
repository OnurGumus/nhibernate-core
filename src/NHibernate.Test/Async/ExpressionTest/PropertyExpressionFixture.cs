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
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PropertyExpressionFixtureAsync : BaseExpressionFixture
	{
		[Test]
		public async Task SqlStringTestAsync()
		{
			ISession session = factory.OpenSession();
			ICriterion expression = Expression.EqProperty("Address", "Name");
			CreateObjects(typeof (Simple), session);
			SqlString sqlString = await (expression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			string expectedSql = "sql_alias.address = sql_alias.Name";
			CompareSqlStrings(sqlString, expectedSql);
			session.Close();
		}
	}
}
#endif
