#if NET_4_5
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Transform;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3609
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally"};
					await (session.SaveAsync(e2));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task AvgWithConditionalDoesNotThrowAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					MappingEntity mappingEntity = null;
					Assert.DoesNotThrowAsync(async () => await (session.QueryOver<Entity>().SelectList(builder => builder.Select(Projections.Avg(Projections.Conditional(Restrictions.Eq(Projections.Property<Entity>(x => x.Name), "FOO"), Projections.Constant("", NHibernateUtil.String), Projections.Constant(null, NHibernateUtil.String))).WithAlias(() => mappingEntity.Count))).TransformUsing(Transformers.AliasToBean<MappingEntity>()).ListAsync<MappingEntity>()));
				}
		}

		[Test]
		public async Task CountWithConditionalDoesNotThrowAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					MappingEntity mappingEntity = null;
					Assert.DoesNotThrowAsync(async () => await (session.QueryOver<Entity>().SelectList(builder => builder.Select(Projections.Count(Projections.Conditional(Restrictions.Eq(Projections.Property<Entity>(x => x.Name), "FOO"), Projections.Constant("", NHibernateUtil.String), Projections.Constant(null, NHibernateUtil.String))).WithAlias(() => mappingEntity.Count))).TransformUsing(Transformers.AliasToBean<MappingEntity>()).ListAsync<MappingEntity>()));
				}
		}

		[Test]
		public async Task GroupByClauseHasParameterSetAsync()
		{
			if (Dialect is FirebirdDialect)
				Assert.Ignore("Firebird does not support complex group by expressions");
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					MappingEntity mappingEntity = null;
					Assert.DoesNotThrowAsync(async () => await (session.QueryOver<Entity>().SelectList(builder => builder.Select(Projections.GroupProperty(Projections.Conditional(Restrictions.Eq(Projections.Property<Entity>(x => x.Name), ""), Projections.Constant(1), Projections.Constant(2))).WithAlias(() => mappingEntity.Count))).TransformUsing(Transformers.AliasToBean<MappingEntity>()).ListAsync<MappingEntity>()));
				}
		}
	}
}
#endif
