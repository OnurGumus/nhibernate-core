#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1863
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var category = new Category{IsActive = true};
					await (s.SaveAsync(category));
					var customerWithCategory = new Customer{Name = "HasCategory"};
					customerWithCategory.Categories.Add(category);
					await (s.SaveAsync(customerWithCategory));
					var customerWithNoCategory = new Customer{Name = "HasNoCategory"};
					await (s.SaveAsync(customerWithNoCategory));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Customer"));
					await (s.DeleteAsync("from Category"));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanGetCustomerWithCategoryWhenFilterIsEnabledAsync()
		{
			using (ISession session = OpenSession())
			{
				IFilter filter = session.EnableFilter("onlyActive");
				filter.SetParameter("activeFlag", true);
				ICriteria hasCategoryCriteria = session.CreateCriteria(typeof (Customer));
				hasCategoryCriteria.Add(Restrictions.Eq("Name", "HasCategory"));
				IList<Customer> hasCategoryResult = await (hasCategoryCriteria.ListAsync<Customer>());
				Assert.That(hasCategoryResult.Count, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task CanGetCustomerWithCategoryWhenFilterIsDisabledAsync()
		{
			using (ISession session = OpenSession())
			{
				session.DisableFilter("onlyActive");
				ICriteria hasCategoryCriteria = session.CreateCriteria(typeof (Customer));
				hasCategoryCriteria.Add(Restrictions.Eq("Name", "HasCategory"));
				IList<Customer> hasCategoryResult = await (hasCategoryCriteria.ListAsync<Customer>());
				Assert.That(hasCategoryResult.Count, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task CanGetCustomerWithNoCategoryWhenFilterIsEnabledAsync()
		{
			using (ISession session = OpenSession())
			{
				IFilter filter = session.EnableFilter("onlyActive");
				filter.SetParameter("activeFlag", true);
				ICriteria hasNoCategoryCriteria = session.CreateCriteria(typeof (Customer));
				hasNoCategoryCriteria.Add(Restrictions.Eq("Name", "HasNoCategory"));
				IList<Customer> hasNoCategoryResult = await (hasNoCategoryCriteria.ListAsync<Customer>());
				Assert.That(hasNoCategoryResult.Count, Is.EqualTo(1));
			}
		}

		[Test]
		public async Task CanGetCustomerWithNoCategoryWhenFilterIsDisabledAsync()
		{
			using (ISession session = OpenSession())
			{
				session.DisableFilter("onlyActive");
				ICriteria hasNoCategoryCriteria = session.CreateCriteria(typeof (Customer));
				hasNoCategoryCriteria.Add(Restrictions.Eq("Name", "HasNoCategory"));
				IList<Customer> hasNoCategoryResult = await (hasNoCategoryCriteria.ListAsync<Customer>());
				Assert.That(hasNoCategoryResult.Count, Is.EqualTo(1));
			}
		}
	}
}
#endif
