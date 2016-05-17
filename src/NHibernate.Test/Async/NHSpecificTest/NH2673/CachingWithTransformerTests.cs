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
						var query = session.CreateQuery("from Blog b where b.Author = : author").SetString("author", "Gabriel").SetCacheable(true).SetResultTransformer(new DistinctRootEntityResultTransformer()).List<Blog>();
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
						var query = session.QueryOver<Blog>().Where(x => x.Author == "Gabriel").TransformUsing(new DistinctRootEntityResultTransformer()).Cacheable().List<Blog>();
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
						query.List<BlogAuthorDto>();
						await (tx.CommitAsync());
					}

				using (var session = OpenSession())
					using (var tx = session.BeginTransaction())
					{
						var query = session.QueryOver<Blog>().Select(x => x.Author, x => x.Name).Where(x => x.Author == "Gabriel").TransformUsing(transformer).Cacheable().List<BlogAuthorDto>();
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
						var query = session.CreateCriteria<Blog>().SetFetchMode("Posts", FetchMode.Eager).SetCacheable(true).List<Blog>();
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
						var query = session.CreateCriteria<Blog>().SetFetchMode("Posts", FetchMode.Eager).SetCacheable(true).Future<Blog>().ToList();
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
						var query = session.CreateQuery("select b from Blog b join fetch b.Posts where b.Author = : author").SetString("author", "Gabriel").SetCacheable(true).List<Blog>();
						await (tx.CommitAsync());
					}
			}
		}
	}
}
#endif
