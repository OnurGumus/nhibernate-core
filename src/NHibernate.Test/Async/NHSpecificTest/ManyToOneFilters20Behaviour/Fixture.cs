#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.ManyToOneFilters20Behaviour
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private static async Task<IList<Parent>> joinGraphUsingHqlAsync(ISession s)
		{
			const string hql = @"select p from Parent p
                                    join p.Child c";
			return await (s.CreateQuery(hql).ListAsync<Parent>());
		}

		private static async Task<IList<Parent>> joinGraphUsingCriteriaAsync(ISession s)
		{
			return await (s.CreateCriteria(typeof (Parent)).SetFetchMode("Child", FetchMode.Join).ListAsync<Parent>());
		}

		private static Parent createParent()
		{
			var ret = new Parent{Child = new Child()};
			ret.Address = new Address{Parent = ret};
			return ret;
		}

		private static void enableFilters(ISession s)
		{
			IFilter f = s.EnableFilter("activeChild");
			f.SetParameter("active", true);
			IFilter f2 = s.EnableFilter("alwaysValid");
			f2.SetParameter("always", true);
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Parent"));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task VerifyAlwaysFilterAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Parent p = createParent();
					p.Child.Always = false;
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}
			}

			using (ISession s = OpenSession())
			{
				enableFilters(s);
				IList<Parent> resCriteria = await (joinGraphUsingCriteriaAsync(s));
				IList<Parent> resHql = await (joinGraphUsingHqlAsync(s));
				Assert.AreEqual(0, resCriteria.Count);
				Assert.AreEqual(0, resHql.Count);
			}
		}

		[Test]
		public async Task VerifyFilterActiveButNotUsedInManyToOneAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(createParent()));
					await (tx.CommitAsync());
				}
			}

			using (ISession s = OpenSession())
			{
				enableFilters(s);
				IList<Parent> resCriteria = await (joinGraphUsingCriteriaAsync(s));
				IList<Parent> resHql = await (joinGraphUsingHqlAsync(s));
				Assert.AreEqual(1, resCriteria.Count);
				Assert.IsNotNull(resCriteria[0].Child);
				Assert.AreEqual(1, resHql.Count);
				Assert.IsNotNull(resHql[0].Child);
			}
		}

		[Test]
		public async Task VerifyQueryWithWhereClauseAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					var p = createParent();
					p.ParentString = "a";
					p.Child.ChildString = "b";
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}
			}

			IList<Parent> resCriteria;
			IList<Parent> resHql;
			using (ISession s = OpenSession())
			{
				enableFilters(s);
				resCriteria = await (s.CreateCriteria(typeof (Parent), "p").CreateCriteria("Child", "c").SetFetchMode("Child", FetchMode.Join).Add(Restrictions.Eq("p.ParentString", "a")).Add(Restrictions.Eq("c.ChildString", "b")).ListAsync<Parent>());
				resHql = await (s.CreateQuery(@"select p from Parent p
                                                        join fetch p.Child c
                                                    where p.ParentString='a' and c.ChildString='b'").ListAsync<Parent>());
			}

			Assert.AreEqual(1, resCriteria.Count);
			Assert.IsNotNull(resCriteria[0].Child);
			Assert.AreEqual(1, resHql.Count);
			Assert.IsNotNull(resHql[0].Child);
		}

		[Test]
		public async Task VerifyAlwaysFiltersOnPropertyRefAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Parent p = createParent();
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}
			}

			using (ISession s = OpenSession())
			{
				enableFilters(s);
				IList<Parent> resCriteria = await (joinGraphUsingCriteriaAsync(s));
				IList<Parent> resHql = await (joinGraphUsingHqlAsync(s));
				Assert.IsNotNull(resCriteria[0].Address);
				Assert.IsNotNull(resHql[0].Address);
			}
		}

		[Test]
		public async Task ExplicitFiltersOnCollectionsShouldBeActiveAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Parent p = createParent();
					p.Children = new List<Child>{new Child{IsActive = true}, new Child{IsActive = false}, new Child{IsActive = true}};
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}
			}

			using (ISession s = OpenSession())
			{
				IFilter f = s.EnableFilter("active");
				f.SetParameter("active", true);
				IList<Parent> resCriteria = await (joinGraphUsingCriteriaAsync(s));
				IList<Parent> resHql = await (joinGraphUsingHqlAsync(s));
				Assert.AreEqual(2, resCriteria[0].Children.Count);
				Assert.AreEqual(2, resHql[0].Children.Count);
			}
		}

		[Test]
		public async Task ExplicitFiltersOnCollectionsShouldBeActiveWithEagerLoadAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Parent p = createParent();
					p.Children = new List<Child>{new Child{IsActive = true}, new Child{IsActive = false}, new Child{IsActive = true}};
					await (s.SaveAsync(p));
					await (tx.CommitAsync());
				}
			}

			using (ISession s = OpenSession())
			{
				IFilter f = s.EnableFilter("active");
				f.SetParameter("active", true);
				IList<Parent> resCriteria = await (s.CreateCriteria(typeof (Parent)).SetFetchMode("Children", FetchMode.Join).ListAsync<Parent>());
				IList<Parent> resHql = await (s.CreateQuery("select p from Parent p join fetch p.Children").ListAsync<Parent>());
				Assert.AreEqual(2, resCriteria[0].Children.Count);
				Assert.AreEqual(2, resHql[0].Children.Count);
			}
		}
	}
}
#endif
