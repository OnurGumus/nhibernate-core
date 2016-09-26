#if NET_4_5
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH3050
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureByCodeAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Entity>(rc =>
			{
				rc.Id(x => x.Id, m => m.Generator(Generators.GuidComb));
				rc.Property(x => x.Name);
			}

			);
			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					var e1 = new Entity{Name = "Bob"};
					await (session.SaveAsync(e1));
					var e2 = new Entity{Name = "Sally"};
					await (session.SaveAsync(e2));
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

		/// <summary>
		/// Uses reflection to create a new SoftLimitMRUCache with a specified size and sets session factory query plan cache to it.
		/// This is done like this as NHibernate does not currently provide any way to specify the query plan cache size through configuration.
		/// </summary>
		/// <param name = "factory"></param>
		/// <param name = "size"></param>
		/// <returns></returns>
		private static bool TrySetQueryPlanCacheSize(NHibernate.ISessionFactory factory, int size)
		{
			var factoryImpl = factory as NHibernate.Impl.SessionFactoryImpl;
			if (factoryImpl != null)
			{
				var queryPlanCacheFieldInfo = typeof (NHibernate.Impl.SessionFactoryImpl).GetField("queryPlanCache", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
				if (queryPlanCacheFieldInfo != null)
				{
					var queryPlanCache = (NHibernate.Engine.Query.QueryPlanCache)queryPlanCacheFieldInfo.GetValue(factoryImpl);
					var planCacheFieldInfo = typeof (NHibernate.Engine.Query.QueryPlanCache).GetField("planCache", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
					if (planCacheFieldInfo != null)
					{
						var softLimitMRUCache = new NHibernate.Util.SoftLimitMRUCache(size);
						planCacheFieldInfo.SetValue(queryPlanCache, softLimitMRUCache);
						return true;
					}
				}
			}

			return false;
		}
	}
}
#endif
