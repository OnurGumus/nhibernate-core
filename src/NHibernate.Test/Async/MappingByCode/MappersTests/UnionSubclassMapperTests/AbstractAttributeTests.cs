#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode.Impl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.MappersTests.UnionSubclassMapperTests
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AbstractAttributeTestsAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private abstract partial class EntityBase
		{
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private abstract partial class Item : EntityBase
		{
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class InventoryItem : Item
		{
		}

		[Test]
		public void CanSetAbstractAttributeOnAbstractClass()
		{
			var mapping = new HbmMapping();
			var mapper = new UnionSubclassMapper(typeof (Item), mapping);
			Assert.That(mapping.UnionSubclasses[0].abstractSpecified, Is.True);
			Assert.That(mapping.UnionSubclasses[0].@abstract, Is.True);
		}

		[Test]
		public void CanSetAbstractAttributeOnConcreteClass()
		{
			var mapping = new HbmMapping();
			var mapper = new UnionSubclassMapper(typeof (InventoryItem), mapping);
			Assert.That(mapping.UnionSubclasses[0].abstractSpecified, Is.False);
			Assert.That(mapping.UnionSubclasses[0].@abstract, Is.False);
		}
	}
}
#endif
