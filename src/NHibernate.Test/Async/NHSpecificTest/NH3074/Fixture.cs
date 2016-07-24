#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Criterion;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3074
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		private const int Id = 123;
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Animal>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.Assigned));
				rc.Property(x => x.Weight);
			}

			);
			mapper.UnionSubclass<Cat>(x => x.Property(p => p.NumberOfLegs));
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var cat = new Cat{Id = Id, NumberOfLegs = 2, Weight = 100};
					await (s.SaveAsync(cat));
					await (tx.CommitAsync());
				}
		}

		[Test]
		[Ignore("Fails on at least Oracle and PostgreSQL. See NH-3074 and NH-2408.")]
		public async Task HqlCanSetLockModeAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var cats = await (s.CreateQuery("select c from Animal c where c.Id=:id").SetInt32("id", Id).SetLockMode("c", LockMode.Upgrade).ListAsync<Cat>());
					Assert.That(cats, Is.Not.Empty);
				}
		}

		[Test, Ignore("Not fixed yet")]
		public async Task CritriaCanSetLockModeAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var cats = await (s.CreateCriteria<Animal>("c").Add(Restrictions.IdEq(Id)).SetLockMode("c", LockMode.Upgrade).ListAsync<Cat>());
					Assert.That(cats, Is.Not.Empty);
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(await (s.GetAsync<Cat>(Id))));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
