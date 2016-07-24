#if NET_4_5
using System;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3138
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureByCodeAsync : TestCaseMappingByCodeAsync
	{
		protected override Cfg.MappingSchema.HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(ca =>
			{
				ca.Lazy(false);
				ca.Id(x => x.Id, map => map.Generator(Generators.GuidComb));
				ca.Property(x => x.EnglishName);
				ca.Property(x => x.GermanName);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		[Test]
		public async Task PageQueryWithDistinctAndOrderByContainingFunctionWithCommaSeparatedParametersAsync()
		{
			using (var session = OpenSession())
			{
				Assert.DoesNotThrowAsync(async () => await (session.CreateQuery("select distinct e.Id, coalesce(e.EnglishName, e.GermanName) from Entity e order by coalesce(e.EnglishName, e.GermanName) asc").SetFirstResult(10).SetMaxResults(20).ListAsync<Entity>()));
			}
		}

		[Test]
		[Ignore("Failing")]
		public async Task PageQueryWithDistinctAndOrderByContainingAliasedFunctionAsync()
		{
			using (var session = OpenSession())
			{
				Assert.DoesNotThrowAsync(async () => await (session.CreateQuery("select distinct e.Id, coalesce(e.EnglishName, e.GermanName) as LocalizedName from Entity e order by LocalizedName asc").SetFirstResult(10).SetMaxResults(20).ListAsync<Entity>()));
			}
		}
	}
}
#endif
