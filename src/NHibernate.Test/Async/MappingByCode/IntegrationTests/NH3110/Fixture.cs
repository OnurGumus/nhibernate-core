#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH3110
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void CanSetPolymorphism()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Polymorphism(PolymorphismType.Explicit);
			}

			);
			var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
			var entity = mapping.RootClasses[0];
			Assert.AreEqual(entity.polymorphism, HbmPolymorphismType.Explicit);
		}
	}
}
#endif
