#if NET_4_5
using System;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

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
	}
}
#endif
