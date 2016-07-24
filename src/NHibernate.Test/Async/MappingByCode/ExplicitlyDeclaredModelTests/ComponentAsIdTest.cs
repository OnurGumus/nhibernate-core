#if NET_4_5
using System;
using System.Linq;
using NHibernate;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.ExplicitlyDeclaredModelTests
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ComponentAsIdTestAsync
	{
		[Test]
		public void CanHaveSameComponentAsIdMultipleTimesWithDifferentColumnNamesForSameProperty()
		{
			//NH-3650
			var model = new ModelMapper();
			model.AddMapping<Child1Map>();
			model.AddMapping<Child2Map>();
			var mappings = model.CompileMappingForEach(new[]{typeof (Child1), typeof (Child2)});
			var child1Mapping = mappings.ElementAt(0);
			Assert.AreEqual("city1", child1Mapping.RootClasses[0].Properties.OfType<HbmComponent>().First().Properties.OfType<HbmProperty>().Single().column);
			//next one fails
			Assert.AreEqual("key2", child1Mapping.RootClasses[0].CompositeId.Items.OfType<HbmKeyProperty>().Last().column1);
			var child2Mapping = mappings.ElementAt(1);
			Assert.AreEqual("city2", child2Mapping.RootClasses[0].Properties.OfType<HbmComponent>().First().Properties.OfType<HbmProperty>().Single().column);
			Assert.AreEqual("key2__", child2Mapping.RootClasses[0].CompositeId.Items.OfType<HbmKeyProperty>().Last().column1);
		}
	}
}
#endif
