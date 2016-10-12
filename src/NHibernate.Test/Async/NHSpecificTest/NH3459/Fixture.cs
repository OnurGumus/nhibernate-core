#if NET_4_5
using System;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3459
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Order>(rc =>
			{
				rc.Table("Orders");
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
				rc.Set(x => x.OrderLines, m =>
				{
					m.Inverse(true);
					m.Key(k =>
					{
						k.Column("OrderId");
						k.NotNullable(true);
					}

					);
					m.Cascade(Mapping.ByCode.Cascade.All.Include(Mapping.ByCode.Cascade.DeleteOrphans));
					m.Access(Accessor.NoSetter);
				}

				, m => m.OneToMany());
			}

			);
			mapper.Class<OrderLine>(rc =>
			{
				rc.Table("OrderLines");
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Manufacturer);
				rc.ManyToOne(x => x.Order, m => m.Column("OrderId"));
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var o1 = new Order{Name = "Order 1"};
					await (session.SaveAsync(o1));
					var o2 = new Order{Name = "Order 2"};
					await (session.SaveAsync(o2));
					await (session.SaveAsync(new OrderLine{Manufacturer = "Manufacturer 1", Order = o2}));
					var o3 = new Order{Name = "Order 3"};
					await (session.SaveAsync(o3));
					await (session.SaveAsync(new OrderLine{Manufacturer = "Manufacturer 1", Order = o3}));
					await (session.SaveAsync(new OrderLine{Manufacturer = "Manufacturer 2", Order = o3}));
					await (session.SaveAsync(new OrderLine{Manufacturer = "Manufacturer 3", Order = o3}));
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
		public async Task LeftOuterJoinAndGroupByAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await ((
						from o in session.Query<Order>()from ol in o.OrderLines.DefaultIfEmpty()group ol by ol.Manufacturer into grp
							select new
							{
							grp.Key
							}

					).ToListAsync());
					Assert.AreEqual(4, result.Count);
				}
		}

		[Test]
		public async Task LeftOuterJoinWithInnerRestrictionAndGroupByAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await ((
						from o in session.Query<Order>()from ol in o.OrderLines.Where(x => x.Manufacturer == "Manufacturer 1").DefaultIfEmpty()group o by o.Name into grp
							select new
							{
							grp.Key
							}

					).ToListAsync());
					Assert.AreEqual(3, result.Count);
				}
		}

		[Test]
		public async Task LeftOuterJoinWithOuterRestrictionAndGroupByAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await ((
						from o in session.Query<Order>()from ol in o.OrderLines.DefaultIfEmpty().Where(x => x.Manufacturer == "Manufacturer 1")group o by o.Name into grp
							select new
							{
							grp.Key
							}

					).ToListAsync());
					Assert.AreEqual(2, result.Count);
				}
		}

		[Test]
		public async Task LeftOuterJoinWithOutermostRestrictionAndGroupByAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await ((
						from o in session.Query<Order>()from ol in o.OrderLines.DefaultIfEmpty()where ol.Manufacturer == "Manufacturer 1"
						group o by o.Name into grp
							select new
							{
							grp.Key
							}

					).ToListAsync());
					Assert.AreEqual(2, result.Count);
				}
		}

		[Test]
		public async Task InnerJoinAndGroupByAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await ((
						from o in session.Query<Order>()from ol in o.OrderLines
						group ol by ol.Manufacturer into grp
							select new
							{
							grp.Key
							}

					).ToListAsync());
					Assert.AreEqual(3, result.Count);
				}
		}

		[Test]
		public async Task InnerJoinWithRestrictionAndGroupByAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await ((
						from o in session.Query<Order>()from ol in o.OrderLines.Where(x => x.Manufacturer == "Manufacturer 1")group o by o.Name into grp
							select new
							{
							grp.Key
							}

					).ToListAsync());
					Assert.AreEqual(2, result.Count);
				}
		}

		[Test]
		public async Task InnerJoinWithOutermostRestrictionAndGroupByAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var result = await ((
						from o in session.Query<Order>()from ol in o.OrderLines
						where ol.Manufacturer == "Manufacturer 1"
						group o by o.Name into grp
							select new
							{
							grp.Key
							}

					).ToListAsync());
					Assert.AreEqual(2, result.Count);
				}
		}
	}
}
#endif
