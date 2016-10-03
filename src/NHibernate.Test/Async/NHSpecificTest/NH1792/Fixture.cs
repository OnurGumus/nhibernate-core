#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1792
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override Task OnTearDownAsync()
		{
			return DeleteAllAsync();
		}

		/// <summary>
		/// Deletes all the product entities from the persistence medium
		/// </summary>
		private async Task DeleteAllAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction trans = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Product"));
					await (trans.CommitAsync());
				}
			}
		}

		/// <summary>
		/// Creates some product enties to work with
		/// </summary>
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					for (int i = 0; i < 10; i++)
					{
						var prod = new Product{Name = "Product" + i};
						await (session.SaveAsync(prod));
					}

					await (tx.CommitAsync());
				}
			}
		}

		/// <summary>
		/// Verifies that a subquery created as a detached criteria with an order by 
		/// will produce valid sql when the main query does not contain an order by clause
		/// </summary>
		[Test]
		public async Task PageWithDetachedCriteriaSubqueryWithOrderByAsync()
		{
			//create the subquery
			DetachedCriteria subQuery = DetachedCriteria.For<Product>().SetProjection(Projections.Id()).AddOrder(Order.Desc("Name")).SetMaxResults(5);
			using (ISession session = OpenSession())
			{
				IList<Product> results = await (session.CreateCriteria<Product>().Add(Subqueries.PropertyIn("Id", subQuery)).Add(Restrictions.Gt("Id", 0)).SetMaxResults(3).ListAsync<Product>());
				Assert.AreEqual(3, results.Count);
			}
		}

		/// <summary>
		/// Verifies that a subquery created as a raw sql statement with an order by 
		/// will produce valid sql when the main query does not contain an order by clause
		/// </summary>
		[Test]
		public async Task PageWithRawSqlSubqueryWithOrderByAsync()
		{
			using (ISession session = OpenSession())
			{
				string top = "";
				if (Dialect.GetType().Name.StartsWith("MsSql"))
					top = "top 5";
				IList<Product> results = await (session.CreateCriteria<Product>().Add(Expression.Sql("{alias}.Id in (Select " + top + " p.Id from Product p order by Name)")).Add(Restrictions.Gt("Id", 0)).SetMaxResults(3).ListAsync<Product>());
				Assert.AreEqual(3, results.Count);
			}
		}
	}
}
#endif
