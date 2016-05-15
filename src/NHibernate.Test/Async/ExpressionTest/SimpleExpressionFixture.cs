#if NET_4_5
using System;
using NHibernate.DomainModel;
using NHibernate.DomainModel.NHSpecific;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleExpressionFixture : BaseExpressionFixture
	{
		[Test]
		public async Task SimpleSqlStringTestAsync()
		{
			ISession session = factory.OpenSession();
			CreateObjects(typeof (Simple), session);
			ICriterion andExpression = Expression.Eq("Address", "12 Adress");
			SqlString sqlString = await (andExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			string expectedSql = "sql_alias.address = ?";
			CompareSqlStrings(sqlString, expectedSql, 1);
			session.Close();
		}

		[Test]
		public async Task TestQuotingAsync()
		{
			using (ISession session = factory.OpenSession())
			{
				DateTime now = DateTime.Now;
				CreateObjects(typeof (SimpleComponent), session);
				ICriterion andExpression = Expression.Eq("Date", now);
				SqlString sqlString = await (andExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
				string quotedColumn = dialect.QuoteForColumnName("d[at]e_");
				string expectedSql = "sql_alias." + quotedColumn + " = ?";
				CompareSqlStrings(sqlString, expectedSql);
			}
		}

		[Test]
		public async Task SimpleDateExpressionAsync()
		{
			using (ISession session = factory.OpenSession())
			{
				CreateObjects(typeof (Simple), session);
				ICriterion andExpression = Expression.Ge("Date", DateTime.Now);
				SqlString sqlString = await (andExpression.ToSqlStringAsync(criteria, criteriaQuery, new CollectionHelper.EmptyMapClass<string, IFilter>()));
				string expectedSql = "sql_alias.date_ >= ?";
				CompareSqlStrings(sqlString, expectedSql, 1);
			}
		}

		[Test]
		public Task MisspelledPropertyWithNormalizedEntityPersisterAsync()
		{
			try
			{
				MisspelledPropertyWithNormalizedEntityPersister();
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
