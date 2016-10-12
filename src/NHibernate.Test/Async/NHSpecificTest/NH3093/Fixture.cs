#if NET_4_5
using System;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3093
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Product>(cm =>
			{
				cm.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				cm.Property(x => x.Name);
				cm.ManyToOne(x => x.Family);
			}

			);
			mapper.Class<Family>(cm =>
			{
				cm.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				cm.Property(x => x.Name);
				cm.ManyToOne(x => x.Segment);
				cm.Set(x => x.Products, m =>
				{
				}

				, m => m.OneToMany());
				cm.Set(x => x.Cultivations, m =>
				{
				}

				, m => m.OneToMany());
			}

			);
			mapper.Class<Cultivation>(cm =>
			{
				cm.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				cm.Property(x => x.Name);
				cm.ManyToOne(x => x.Family);
			}

			);
			mapper.Class<Segment>(cm =>
			{
				cm.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				cm.Property(x => x.Name);
				cm.Set(x => x.Families, m =>
				{
				}

				, m => m.OneToMany());
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var s = new Segment{Name = "segment 1"};
					await (session.SaveAsync(s));
					var f = new Family{Name = "fam 1", Segment = s};
					await (session.SaveAsync(f));
					var c = new Cultivation{Name = "Sample", Family = f};
					await (session.SaveAsync(c));
					var p1 = new Product{Name = "product 1", Family = f};
					await (session.SaveAsync(p1));
					var p2 = new Product{Name = "product 2"};
					await (session.SaveAsync(p2));
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
		public async Task Linq11Async()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var cultivationsId = session.Query<Cultivation>().Select(c => c.Id).ToArray();
					var products = await ((
						from p in session.Query<Product>()where p.Family.Cultivations.Any(c => cultivationsId.Contains(c.Id)) && p.Family.Segment.Name == "segment 1"
						select p).ToListAsync());
					Assert.AreEqual(1, products.Count);
				}
		}

		[Test]
		public async Task Linq2Async()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var products = await ((
						from p in session.Query<Product>()where p.Family.Cultivations.Any(c => c.Name == "Sample") && p.Family.Segment.Name == "segment 1"
						select p).ToListAsync());
					Assert.AreEqual(1, products.Count);
				}
		}
	}
}
#endif
