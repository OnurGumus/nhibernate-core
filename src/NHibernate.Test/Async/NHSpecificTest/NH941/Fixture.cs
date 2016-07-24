#if NET_4_5
using System.Collections.Generic;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH941
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<MyClass>(rc =>
			{
				rc.Id(x => x.Id, map => map.Generator(Generators.HighLow));
				rc.Bag(x => x.Relateds, map =>
				{
					map.Key(km => km.NotNullable(true));
					map.Cascade(Mapping.ByCode.Cascade.All);
				}

				, rel => rel.OneToMany());
			}

			);
			mapper.Class<Related>(rc => rc.Id(x => x.Id, map => map.Generator(Generators.HighLow)));
			HbmMapping mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
			return mappings;
		}

		[Test]
		public async Task WhenSaveOneThenShouldSaveManyAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var one = new MyClass();
					one.Relateds = new List<Related>{new Related(), new Related()};
					await (session.PersistAsync(one));
					await (tx.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from Related").ExecuteUpdateAsync());
					await (session.CreateQuery("delete from MyClass").ExecuteUpdateAsync());
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
