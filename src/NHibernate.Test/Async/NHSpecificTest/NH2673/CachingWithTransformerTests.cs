#if NET_4_5
using System;
using System.Collections;
using System.Linq;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2673
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CachingWithTransformerTests : TestCaseMappingByCode
	{
		[Test]
		public async Task WhenQueryThenNotThrowsAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
					using (var tx = session.BeginTransaction())
					{
						var query = await (session.CreateQuery("from Blog b where b.Author = : author").SetString("author", "Gabriel").SetCacheable(true).SetResultTransformer(new DistinctRootEntityResultTransformer()).ListAsync<Blog>());
						await (tx.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenCriteriaThenNotThrowsAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
					using (var tx = session.BeginTransaction())
					{
						var query = await (session.QueryOver<Blog>().Where(x => x.Author == "Gabriel").TransformUsing(new DistinctRootEntityResultTransformer()).Cacheable().ListAsync<Blog>());
						await (tx.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenCriteriaProjectionThenNotThrowsAsync()
		{
			// during the fix of NH-2673 was faund another bug related to cacheability of criteria with projection + transformer 
			// then found reported as NH-1090
			var transformer = new BlogAuthorTransformer();
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
					using (var tx = session.BeginTransaction())
					{
						var query = session.QueryOver<Blog>().Select(x => x.Author, x => x.Name).Where(x => x.Author == "Gabriel").TransformUsing(transformer).Cacheable();
						await (query.ListAsync<BlogAuthorDto>());
						await (tx.CommitAsync());
					}

				using (var session = OpenSession())
					using (var tx = session.BeginTransaction())
					{
						var query = await (session.QueryOver<Blog>().Select(x => x.Author, x => x.Name).Where(x => x.Author == "Gabriel").TransformUsing(transformer).Cacheable().ListAsync<BlogAuthorDto>());
						await (tx.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenEagerLoadingWithCriteriaThenNotThrowsAsync()
		{
			// reported in dev-list instead on JIRA
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
					using (var tx = session.BeginTransaction())
					{
						var query = await (session.CreateCriteria<Blog>().SetFetchMode("Posts", FetchMode.Eager).SetCacheable(true).ListAsync<Blog>());
						await (tx.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenEagerLoadingWithMultiCriteriaThenNotThrowsAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
					using (var tx = session.BeginTransaction())
					{
						var query = (await (session.CreateCriteria<Blog>().SetFetchMode("Posts", FetchMode.Eager).SetCacheable(true).FutureAsync<Blog>())).ToList();
						await (tx.CommitAsync());
					}
			}
		}

		[Test]
		public async Task WhenEagerLoadingWithHqlThenNotThrowsAsync()
		{
			using (new Scenario(Sfi))
			{
				using (var session = OpenSession())
					using (var tx = session.BeginTransaction())
					{
						var query = await (session.CreateQuery("select b from Blog b join fetch b.Posts where b.Author = : author").SetString("author", "Gabriel").SetCacheable(true).ListAsync<Blog>());
						await (tx.CommitAsync());
					}
			}
		}

		[Test(Description = "NH2961/3311")]
		public async Task CanCacheCriteriaWithLeftJoinAndResultTransformerAsync()
		{
			Post posts = null;
			using (new Scenario(Sfi))
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var result = await (session.QueryOver<Blog>().Where(x => x.Author == "Gabriel").Left.JoinAlias(x => x.Posts, () => posts).TransformUsing(new DistinctRootEntityResultTransformer()).Cacheable().ListAsync<Blog>());
					}
		}

		[Test(Description = "NH2961/3311")]
		public async Task CanCacheCriteriaWithEagerLoadAndResultTransformerAsync()
		{
			using (new Scenario(Sfi))
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var result = await (session.QueryOver<Blog>().Where(x => x.Author == "Gabriel").Fetch(x => x.Posts).Eager.TransformUsing(new DistinctRootEntityResultTransformer()).Cacheable().ListAsync<Blog>());
					}
		}

		[Test(Description = "NH2961/3311")]
		public async Task CanCacheCriteriaWithLeftJoinAsync()
		{
			Post posts = null;
			// Begins to work in 6e21608bbdec096558da956b9df41ab1d63dbd85.
			// Same as CanCacheCriteriaWithLeftJoinAndResultTransformer() but without
			// result transformer.
			using (new Scenario(Sfi))
				using (var session = OpenSession())
					using (session.BeginTransaction())
					{
						var query = await (session.QueryOver<Blog>().Where(x => x.Author == "Gabriel").Left.JoinAlias(x => x.Posts, () => posts).Cacheable().ListAsync<Blog>());
					}
		}
	}
}
#endif
