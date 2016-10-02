#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class SchemaMetadataUpdater
	{
		public static async Task UpdateAsync(ISessionFactory sessionFactory)
		{
			var factory = (ISessionFactoryImplementor)sessionFactory;
			var dialect = factory.Dialect;
			var connectionHelper = new SuppliedConnectionProviderConnectionHelper(factory.ConnectionProvider);
			factory.Dialect.Keywords.UnionWith(await (GetReservedWordsAsync(dialect, connectionHelper)));
		}

		public static async Task QuoteTableAndColumnsAsync(Configuration configuration)
		{
			ISet<string> reservedDb = await (GetReservedWordsAsync(configuration.GetDerivedProperties()));
			foreach (var cm in configuration.ClassMappings)
			{
				QuoteTable(cm.Table, reservedDb);
			}

			foreach (var cm in configuration.CollectionMappings)
			{
				QuoteTable(cm.Table, reservedDb);
			}
		}

		private static async Task<ISet<string>> GetReservedWordsAsync(IDictionary<string, string> cfgProperties)
		{
			var dialect = Dialect.Dialect.GetDialect(cfgProperties);
			var connectionHelper = new ManagedProviderConnectionHelper(cfgProperties);
			return await (GetReservedWordsAsync(dialect, connectionHelper));
		}

		private static async Task<ISet<string>> GetReservedWordsAsync(Dialect.Dialect dialect, IConnectionHelper connectionHelper)
		{
			ISet<string> reservedDb = new HashSet<string>();
			await (connectionHelper.PrepareAsync());
			try
			{
				var metaData = dialect.GetDataBaseSchema(connectionHelper.Connection);
				foreach (var rw in metaData.GetReservedWords())
				{
					reservedDb.Add(rw.ToLowerInvariant());
				}
			}
			finally
			{
				connectionHelper.Release();
			}

			return reservedDb;
		}
	}
}
#endif
