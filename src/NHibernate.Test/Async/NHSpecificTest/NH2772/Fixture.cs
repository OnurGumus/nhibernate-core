#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2772
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					var trip = new Trip();
					trip.Header = "Header1";
					var tp1 = trip.CreateTrackpoint();
					tp1.Lat = 1;
					tp1.Lon = 1;
					var tp2 = trip.CreateTrackpoint();
					tp2.Lat = 2;
					tp2.Lon = 2;
					var tp3 = trip.CreateTrackpoint();
					tp3.Lat = 3;
					tp3.Lon = 3;
					await (s.SaveAsync(trip));
					await (t.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var t = s.BeginTransaction())
				{
					await (s.DeleteAsync("from System.Object"));
					await (t.CommitAsync());
				}
		}

		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Trip>(rc =>
			{
				rc.Id(x => x.Id, map => map.Generator(Generators.Identity));
				rc.Property(x => x.Header, map => map.NotNullable(true));
				rc.Property(x => x.Image, map => map.Lazy(true)); // This will make the test fail
				rc.Bag(x => x.Trackpoints, map =>
				{
					map.Key(x => x.Column("TripId"));
					map.Access(Accessor.Field);
					map.Cascade(Mapping.ByCode.Cascade.All);
					map.BatchSize(10);
					map.Lazy(CollectionLazy.Lazy);
					map.Inverse(true);
				}

				, rel => rel.OneToMany());
			}

			);
			mapper.Class<Trackpoint>(rc =>
			{
				rc.Id(x => x.Id, map => map.Generator(Generators.Identity));
				rc.Property(x => x.Lat, map => map.NotNullable(true));
				rc.Property(x => x.Lon, map => map.NotNullable(true));
				rc.ManyToOne(x => x.Trip, map =>
				{
					map.Column("TripId");
					map.Cascade(Mapping.ByCode.Cascade.None);
				}

				);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		[Test]
		public async Task Lazy_Collection_Is_Not_LoadedAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var trip = await (s.GetAsync<Trip>(1));
					Assert.That(trip.Trackpoints.Count(), Is.EqualTo(3));
				}
		}
	}
}
#endif
