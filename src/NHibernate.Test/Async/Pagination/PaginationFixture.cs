#if NET_4_5
using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.Pagination
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PaginationFixture : TestCase
	{
		[Test]
		public async Task PagTestAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					for (int i = 0; i < 10; i++)
					{
						var dp = new DataPoint{X = (i * 0.1d)};
						dp.Y = Math.Cos(dp.X);
						await (s.PersistAsync(dp));
					}

					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					int size = s.CreateSQLQuery("select Id, xval, yval, Description from DataPoint order by xval, yval").AddEntity(typeof (DataPoint)).SetMaxResults(5).List().Count;
					Assert.That(size, Is.EqualTo(5));
					size = s.CreateQuery("from DataPoint dp order by dp.X, dp.Y").SetFirstResult(5).SetMaxResults(2).List().Count;
					Assert.That(size, Is.EqualTo(2));
					size = s.CreateCriteria(typeof (DataPoint)).AddOrder(Order.Asc("X")).AddOrder(Order.Asc("Y")).SetFirstResult(8).List().Count;
					Assert.That(size, Is.EqualTo(2));
					size = s.CreateCriteria(typeof (DataPoint)).AddOrder(Order.Asc("X")).AddOrder(Order.Asc("Y")).SetMaxResults(10).SetFirstResult(8).List().Count;
					Assert.That(size, Is.EqualTo(2));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from DataPoint"));
					await (t.CommitAsync());
				}
		}

		[Test]
		public async Task PagingWithLock_NH2255Async()
		{
			if (Dialect is Oracle12cDialect)
				Assert.Ignore(@"Oracle does not support row_limiting_clause with for_update_clause
See: https://docs.oracle.com/database/121/SQLRF/statements_10002.htm#BABHFGAA");
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					await (s.SaveAsync(new DataPoint()
					{X = 4}));
					await (s.SaveAsync(new DataPoint()
					{X = 5}));
					await (s.SaveAsync(new DataPoint()
					{X = 6}));
					await (s.SaveAsync(new DataPoint()
					{X = 7}));
					await (s.SaveAsync(new DataPoint()
					{X = 8}));
					await (t.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				var points = s.CreateCriteria<DataPoint>().Add(Restrictions.Gt("X", 4.1d)).AddOrder(Order.Asc("X")).SetLockMode(LockMode.Upgrade).SetFirstResult(1).SetMaxResults(2).List<DataPoint>();
				Assert.That(points.Count, Is.EqualTo(2));
				Assert.That(points[0].X, Is.EqualTo(6d));
				Assert.That(points[1].X, Is.EqualTo(7d));
			}

			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					s.CreateQuery("delete from DataPoint").ExecuteUpdate();
					await (t.CommitAsync());
				}
		}
	}
}
#endif
