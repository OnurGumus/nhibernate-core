#if NET_4_5
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.MappingByCode.ExplicitMappingTests
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OptimisticLockModeTestsAsync
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class MyClass
		{
			public int Id
			{
				get;
				set;
			}

			public int Version
			{
				get;
				set;
			}
		}

		[Test]
		public void OptimisticLockModeTest()
		{
			//NH-2823
			var mapper = new ModelMapper();
			mapper.Class<MyClass>(map =>
			{
				map.Id(x => x.Id, idmap =>
				{
				}

				);
				map.OptimisticLock(OptimisticLockMode.Dirty);
			}

			);
			var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
			Assert.AreEqual(mappings.RootClasses[0].optimisticlock, HbmOptimisticLockMode.Dirty);
		}
	}
}
#endif
