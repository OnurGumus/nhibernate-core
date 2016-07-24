#if NET_4_5
using System.Linq;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2251
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task WhenUseFutureSkipTakeThenNotThrowAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.QueryOver<Foo>().Where(o => o.Name == "Graeme");
				var rowcountQuery = await (query.ToRowCountQuery().FutureValueAsync<int>());
				var resultsQuery = await (query.Skip(0).Take(50).FutureAsync());
				int rowcount;
				Foo[] items;
				Assert.That(() =>
				{
					rowcount = rowcountQuery.Value;
					items = resultsQuery.ToArray();
				}

				, Throws.Nothing);
			}
		}

		[Test]
		public async Task EnlistingFirstThePaginationAndThenTheRowCountDoesNotThrowsAsync()
		{
			using (var session = OpenSession())
			{
				var query = session.QueryOver<Foo>().Where(o => o.Name == "Graeme");
				var resultsQuery = await (query.Skip(0).Take(50).FutureAsync());
				var rowcountQuery = await (query.ToRowCountQuery().FutureValueAsync<int>());
				int rowcount;
				Foo[] items;
				Assert.That(() =>
				{
					rowcount = rowcountQuery.Value;
					items = resultsQuery.ToArray();
				}

				, Throws.Nothing);
			}
		}

		[Test]
		public async Task HqlWithOffsetAndLimitAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Foo()
					{Name = "name1"}));
					await (session.SaveAsync(new Foo()
					{Name = "name2"}));
					await (session.SaveAsync(new Foo()
					{Name = "name3"}));
					await (session.SaveAsync(new Foo()
					{Name = "name4"}));
					string stringParam = "name%";
					var list = await (session.CreateQuery("from Foo f where f.Name like :stringParam order by f.Name").SetParameter("stringParam", stringParam).SetFirstResult(1).SetMaxResults(2).ListAsync<Foo>());
					Assert.That(list.Count(), Is.EqualTo(2));
					Assert.That(list[0].Name, Is.EqualTo("name2"));
					Assert.That(list[1].Name, Is.EqualTo("name3"));
				}
		}

		[Test]
		public async Task FuturePagedHqlAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Foo()
					{Name = "name1"}));
					await (session.SaveAsync(new Foo()
					{Name = "name2"}));
					await (session.SaveAsync(new Foo()
					{Name = "name3"}));
					await (session.SaveAsync(new Foo()
					{Name = "name4"}));
					string stringParam = "name%";
					var list1 = await (session.CreateQuery("from Foo f where f.Name like :stringParam order by f.Name").SetParameter("stringParam", stringParam).SetFirstResult(1).SetMaxResults(2).FutureAsync<Foo>());
					var list2 = await (session.CreateQuery("from Foo f where f.Name like :stringParam order by f.Name").SetParameter("stringParam", stringParam).SetFirstResult(1).SetMaxResults(2).FutureAsync<Foo>());
					Assert.That(list1.Count(), Is.EqualTo(2));
					Assert.That(list1.ElementAt(0).Name, Is.EqualTo("name2"));
					Assert.That(list1.ElementAt(1).Name, Is.EqualTo("name3"));
					Assert.That(list2.Count(), Is.EqualTo(2));
					Assert.That(list2.ElementAt(0).Name, Is.EqualTo("name2"));
					Assert.That(list2.ElementAt(1).Name, Is.EqualTo("name3"));
				}
		}

		[Test]
		public async Task MultiplePagingParametersInSingleQueryAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Foo()
					{Ord = 0, Name = "00"}));
					await (session.SaveAsync(new Foo()
					{Ord = 1, Name = "10"}));
					await (session.SaveAsync(new Foo()
					{Ord = 2, Name = "10"}));
					await (session.SaveAsync(new Foo()
					{Ord = 3, Name = "11"}));
					await (session.SaveAsync(new Foo()
					{Ord = 4, Name = "10"}));
					await (session.SaveAsync(new Foo()
					{Ord = 5, Name = "10"}));
					await (session.SaveAsync(new Foo()
					{Ord = 6, Name = "10"}));
					await (session.SaveAsync(new Foo()
					{Ord = 7, Name = "10"}));
					await (session.SaveAsync(new Foo()
					{Ord = 8, Name = "10"}));
					await (session.SaveAsync(new Foo()
					{Ord = 9, Name = "10"}));
					// returns 2, 3, 4, 5, 6, 7, 8
					DetachedCriteria pagedSubquery = DetachedCriteria.For<Foo>().Add(Restrictions.Like("Name", "1%")).AddOrder(Order.Asc("Ord")).SetFirstResult(1).SetMaxResults(7).SetProjection(Projections.Property("Id"));
					var query = session.CreateCriteria<Foo>().Add(Subqueries.PropertyIn("Id", pagedSubquery)).Add(Restrictions.Like("Name", "%0")) // excludes 3
					.AddOrder(Order.Asc("Ord")).SetFirstResult(2).SetMaxResults(3);
					var list = await (query.ListAsync<Foo>());
					Assert.That(list.Count, Is.EqualTo(3));
					Assert.That(list[0].Ord, Is.EqualTo(5));
					Assert.That(list[1].Ord, Is.EqualTo(6));
					Assert.That(list[2].Ord, Is.EqualTo(7));
				}
		}
	}
}
#endif
