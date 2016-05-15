#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2195
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SQLiteMultiCriteriaTest : BugTestCase
	{
		[Test]
		public async Task SingleCriteriaQueriesWithIntsShouldExecuteCorrectlyAsync()
		{
			// Test querying IntData
			using (ISession session = this.OpenSession())
			{
				ICriteria criteriaWithPagination = session.CreateCriteria<DomainClass>();
				criteriaWithPagination.Add(Expression.Le("IntData", 2));
				ICriteria criteriaWithRowCount = CriteriaTransformer.Clone(criteriaWithPagination);
				criteriaWithPagination.SetFirstResult(0).SetMaxResults(1);
				criteriaWithRowCount.SetProjection(Projections.RowCountInt64());
				IList<DomainClass> list = await (criteriaWithPagination.ListAsync<DomainClass>());
				Assert.AreEqual(2, await (criteriaWithRowCount.UniqueResultAsync<long>()));
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task SingleCriteriaQueriesWithStringsShouldExecuteCorrectlyAsync()
		{
			// Test querying StringData
			using (ISession session = this.OpenSession())
			{
				ICriteria criteriaWithPagination = session.CreateCriteria<DomainClass>();
				criteriaWithPagination.Add(Expression.Like("StringData", "%Doe%"));
				ICriteria criteriaWithRowCount = CriteriaTransformer.Clone(criteriaWithPagination);
				criteriaWithPagination.SetFirstResult(0).SetMaxResults(1);
				criteriaWithRowCount.SetProjection(Projections.RowCountInt64());
				IList<DomainClass> list = await (criteriaWithPagination.ListAsync<DomainClass>());
				Assert.AreEqual(2, await (criteriaWithRowCount.UniqueResultAsync<long>()));
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task MultiCriteriaQueriesWithIntsShouldExecuteCorrectlyAsync()
		{
			var driver = sessions.ConnectionProvider.Driver;
			if (!driver.SupportsMultipleQueries)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);
			// Test querying IntData
			using (ISession session = this.OpenSession())
			{
				ICriteria criteriaWithPagination = session.CreateCriteria<DomainClass>();
				criteriaWithPagination.Add(Expression.Le("IntData", 2));
				ICriteria criteriaWithRowCount = CriteriaTransformer.Clone(criteriaWithPagination);
				criteriaWithPagination.SetFirstResult(0).SetMaxResults(1);
				criteriaWithRowCount.SetProjection(Projections.RowCountInt64());
				IMultiCriteria multiCriteria = session.CreateMultiCriteria();
				multiCriteria.Add(criteriaWithPagination);
				multiCriteria.Add(criteriaWithRowCount);
				IList results = await (multiCriteria.ListAsync());
				long numResults = (long)((IList)results[1])[0];
				IList list = (IList)results[0];
				Assert.AreEqual(2, await (criteriaWithRowCount.UniqueResultAsync<long>()));
				Assert.AreEqual(1, list.Count);
			}
		}

		[Test]
		public async Task MultiCriteriaQueriesWithStringsShouldExecuteCorrectlyAsync()
		{
			var driver = sessions.ConnectionProvider.Driver;
			if (!driver.SupportsMultipleQueries)
				Assert.Ignore("Driver {0} does not support multi-queries", driver.GetType().FullName);
			// Test querying StringData
			using (ISession session = this.OpenSession())
			{
				ICriteria criteriaWithPagination = session.CreateCriteria<DomainClass>();
				criteriaWithPagination.Add(Expression.Like("StringData", "%Doe%"));
				ICriteria criteriaWithRowCount = CriteriaTransformer.Clone(criteriaWithPagination);
				criteriaWithPagination.SetFirstResult(0).SetMaxResults(1);
				criteriaWithRowCount.SetProjection(Projections.RowCountInt64());
				IMultiCriteria multiCriteria = session.CreateMultiCriteria();
				multiCriteria.Add(criteriaWithPagination);
				multiCriteria.Add(criteriaWithRowCount);
				IList results = await (multiCriteria.ListAsync());
				long numResults = (long)((IList)results[1])[0];
				IList list = (IList)results[0];
				Assert.AreEqual(2, await (criteriaWithRowCount.UniqueResultAsync<long>()));
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
#endif
