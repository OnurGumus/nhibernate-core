#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.CfgTest.Loquacious
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityCacheConfigurationFixtureAsync
	{
		[Test]
		public void ConfigureCacheOfClass()
		{
			Configuration configure = new Configuration().Configure();
			configure.AddResource("NHibernate.Test.CfgTest.Loquacious.EntityToCache.hbm.xml", GetType().Assembly);
			configure.EntityCache<EntityToCache>(ce =>
			{
				ce.Strategy = EntityCacheUsage.NonStrictReadWrite;
				ce.RegionName = "MyRegion";
			}

			);
			var pc = (RootClass)configure.GetClassMapping(typeof (EntityToCache));
			Assert.That(pc.CacheConcurrencyStrategy, Is.EqualTo(EntityCacheUsageParser.ToString(EntityCacheUsage.NonStrictReadWrite)));
			Assert.That(pc.CacheRegionName, Is.EqualTo("MyRegion"));
		}

		[Test]
		public void ConfigureCacheOfCollection()
		{
			Configuration configure = new Configuration().Configure();
			configure.AddResource("NHibernate.Test.CfgTest.Loquacious.EntityToCache.hbm.xml", GetType().Assembly);
			configure.EntityCache<EntityToCache>(ce =>
			{
				ce.Strategy = EntityCacheUsage.NonStrictReadWrite;
				ce.RegionName = "MyRegion";
				ce.Collection(e => e.Elements, cc =>
				{
					cc.RegionName = "MyCollectionRegion";
					cc.Strategy = EntityCacheUsage.NonStrictReadWrite;
				}

				);
			}

			);
			Mapping.Collection pc = configure.GetCollectionMapping("NHibernate.Test.CfgTest.Loquacious.EntityToCache.Elements");
			Assert.That(pc.CacheConcurrencyStrategy, Is.EqualTo(EntityCacheUsageParser.ToString(EntityCacheUsage.NonStrictReadWrite)));
			Assert.That(pc.CacheRegionName, Is.EqualTo("MyCollectionRegion"));
		}

		[Test]
		public void ConfigureCacheOfCollectionWithOutEntity()
		{
			Configuration configure = new Configuration().Configure();
			configure.AddResource("NHibernate.Test.CfgTest.Loquacious.EntityToCache.hbm.xml", GetType().Assembly);
			configure.EntityCache<EntityToCache>(ce => ce.Collection(e => e.Elements, cc =>
			{
				cc.RegionName = "MyCollectionRegion";
				cc.Strategy = EntityCacheUsage.NonStrictReadWrite;
			}

			));
			var pc = (RootClass)configure.GetClassMapping(typeof (EntityToCache));
			Assert.That(pc.CacheConcurrencyStrategy, Is.Null);
		}

		[Test]
		public Task NotAllowRelatedCollectionsAsync()
		{
			try
			{
				Configuration configure = new Configuration().Configure();
				configure.AddResource("NHibernate.Test.CfgTest.Loquacious.EntityToCache.hbm.xml", GetType().Assembly);
				var exception = Assert.Throws<ArgumentOutOfRangeException>(() => configure.EntityCache<EntityToCache>(ce => ce.Collection(e => e.Relation.Elements, cc =>
				{
				}

				)));
				Assert.That(exception.Message, Is.StringContaining("Collection not owned by"));
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
