#if NET_4_5
using System;
using NHibernate.Cache;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2510
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseMappingByCodeAsync
	{
		protected override HbmMapping GetMappings()
		{
			var mapper = new ModelMapper();
			mapper.Class<Image>(rc =>
			{
				rc.Cache(map => map.Usage(CacheUsage.NonstrictReadWrite));
				rc.Id(x => x.Id);
				rc.Property(x => x.Data, map => map.Lazy(true));
			}

			);
			var mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();
			return mappings;
		}

		protected override Task ConfigureAsync(Cfg.Configuration configuration)
		{
			try
			{
				configuration.Cache(x => x.Provider<HashtableCacheProvider>());
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private class Scenario : IDisposable
		{
			private readonly ISessionFactory factory;
			public Scenario(ISessionFactory factory)
			{
				this.factory = factory;
				using (var session = factory.OpenSession())
					using (session.BeginTransaction())
					{
						session.Persist(new Image{Id = 1});
						session.Transaction.Commit();
					}
			}

			public void Dispose()
			{
				using (var session = factory.OpenSession())
					using (session.BeginTransaction())
					{
						session.CreateQuery("delete from Image").ExecuteUpdate();
						session.Transaction.Commit();
					}
			}
		}

		[Test]
		public async Task WhenReadFromCacheThenDoesNotThrowAsync()
		{
			using (new Scenario(Sfi))
			{
				using (ISession s = OpenSession())
				{
					var book = await (s.GetAsync<Image>(1));
				}

				using (ISession s = OpenSession())
				{
					var book = await (s.GetAsync<Image>(1));
				}
			}
		}
	}
}
#endif
