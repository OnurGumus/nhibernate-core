#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3237
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByCodeFixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.DateTimeOffsetValue, m => m.Type(typeof (DateTimeOffsetUserType), new DateTimeOffsetUserType(TimeSpan.FromHours(10))));
				rc.Property(x => x.EnumValue, m => m.Type(typeof (EnumUserType), null));
				rc.Property(x => x.IntValue);
				rc.Property(x => x.LongValue);
				rc.Property(x => x.DecimalValue);
				rc.Property(x => x.DoubleValue);
				rc.Property(x => x.FloatValue);
				rc.Property(x => x.DateTimeValue);
				rc.Property(x => x.StringValue);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{DateTimeOffsetValue = new DateTimeOffset(2012, 08, 06, 11, 0, 0, TimeSpan.FromHours(10)), EnumValue = TestEnum.Zero, IntValue = 1, LongValue = 1L, DecimalValue = 1.2m, DoubleValue = 1.2d, FloatValue = 1.2f, DateTimeValue = new DateTime(2012, 08, 06, 11, 0, 0), StringValue = "a"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{DateTimeOffsetValue = new DateTimeOffset(2012, 08, 06, 12, 0, 0, TimeSpan.FromHours(10)), EnumValue = TestEnum.One, IntValue = 2, LongValue = 2L, DecimalValue = 2.2m, DoubleValue = 2.2d, FloatValue = 2.2f, DateTimeValue = new DateTime(2012, 08, 06, 12, 0, 0), StringValue = "b"};
					await (session.SaveAsync(e2));
					var e3 = new Entity{DateTimeOffsetValue = new DateTimeOffset(2012, 08, 06, 13, 0, 0, TimeSpan.FromHours(10)), EnumValue = TestEnum.Two, IntValue = 3, LongValue = 3L, DecimalValue = 3.2m, DoubleValue = 3.2d, FloatValue = 3.2f, DateTimeValue = new DateTime(2012, 08, 06, 13, 0, 0), StringValue = "c"};
					await (session.SaveAsync(e3));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (session.FlushAsync());
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
