#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1776
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FilterQueryTwiceFixture : BugTestCase
	{
		// Note : in this test what is really important is the usage of the same HQL
		// because QueryPlan
		[Test]
		[Description("Can Query using Session's filter Twice")]
		public async Task BugAsync()
		{
			var c = new Category{Code = "2600", Deleted = false};
			await (SaveCategoryAsync(c));
			// exec queries, twice, different session
			ExecQuery();
			ExecQuery();
			// cleanup using filter
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					s.EnableFilter("state").SetParameter("deleted", false);
					await (s.DeleteAsync("from Category"));
					await (tx.CommitAsync());
				}
			}
		}

		private async Task SaveCategoryAsync(Category c)
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(c));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		[Description("Executing same query with and without filter and with different filter parameter value.")]
		public async Task FilterOnOffOnAsync()
		{
			var c = new Category{Code = "2600", Deleted = true};
			await (SaveCategoryAsync(c));
			using (ISession s = OpenSession())
			{
				s.EnableFilter("state").SetParameter("deleted", false);
				IList<Category> result = s.CreateQuery("from Category where Code = :code").SetParameter("code", "2600").List<Category>();
				Assert.That(result.Count == 0);
			}

			using (ISession s = OpenSession())
			{
				IList<Category> result = s.CreateQuery("from Category where Code = :code").SetParameter("code", "2600").List<Category>();
				Assert.That(result.Count > 0);
			}

			using (ISession s = OpenSession())
			{
				s.EnableFilter("state").SetParameter("deleted", true);
				IList<Category> result = s.CreateQuery("from Category where Code = :code").SetParameter("code", "2600").List<Category>();
				Assert.That(result.Count > 0);
			}

			await (CleanupAsync());
		}

		private async Task CleanupAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					s.CreateQuery("delete from Category").ExecuteUpdate();
					await (tx.CommitAsync());
				}
		}

		[Test]
		[Description("Executing same query with different filters combinations.")]
		public async Task MultiFilterOnOffOnAsync()
		{
			var c = new Category{Code = "2600", Deleted = true};
			await (SaveCategoryAsync(c));
			using (ISession s = OpenSession())
			{
				s.EnableFilter("state").SetParameter("deleted", false);
				IList<Category> result = s.CreateQuery("from Category where Code = :code").SetParameter("code", "2600").List<Category>();
				Assert.That(result.Count == 0);
			}

			using (ISession s = OpenSession())
			{
				s.EnableFilter("state").SetParameter("deleted", true);
				s.EnableFilter("CodeLike").SetParameter("codepattern", "2%");
				IList<Category> result = s.CreateQuery("from Category where Code = :code").SetParameter("code", "NotExists").List<Category>();
				Assert.That(result.Count == 0);
			}

			using (ISession s = OpenSession())
			{
				s.EnableFilter("CodeLike").SetParameter("codepattern", "2%");
				IList<Category> result = s.CreateQuery("from Category where Code = :code").SetParameter("code", "2600").List<Category>();
				Assert.That(result.Count > 0);
			}

			await (CleanupAsync());
		}
	}
}
#endif
