using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	// Candidate to be exstensions of ISessionFactory and Configuration
	public static class SchemaMetadataUpdater
	{
		public static void Update(ISessionFactory sessionFactory)
		{
			UpdateAsync(sessionFactory).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public static async Task UpdateAsync(ISessionFactory sessionFactory)
		{
			var factory = (ISessionFactoryImplementor) sessionFactory;
			var dialect = factory.Dialect;
			var connectionHelper = new SuppliedConnectionProviderConnectionHelper(factory.ConnectionProvider);
			factory.Dialect.Keywords.UnionWith(await GetReservedWords(dialect, connectionHelper).ConfigureAwait(false));
		}

		public static void QuoteTableAndColumns(Configuration configuration)
		{
			QuoteTableAndColumnsAsync(configuration).ConfigureAwait(false).GetAwaiter().GetResult();
		}

		public static async Task QuoteTableAndColumnsAsync(Configuration configuration)
		{
			ISet<string> reservedDb = await GetReservedWords(configuration.GetDerivedProperties()).ConfigureAwait(false);
			foreach (var cm in configuration.ClassMappings)
			{
				QuoteTable(cm.Table, reservedDb);
			}
			foreach (var cm in configuration.CollectionMappings)
			{
				QuoteTable(cm.Table, reservedDb);
			}
		}

		private static Task<ISet<string>> GetReservedWords(IDictionary<string, string> cfgProperties)
		{
			var dialect = Dialect.Dialect.GetDialect(cfgProperties);
			var connectionHelper = new ManagedProviderConnectionHelper(cfgProperties);
			return GetReservedWords(dialect, connectionHelper);
		}

		private static async Task<ISet<string>> GetReservedWords(Dialect.Dialect dialect, IConnectionHelper connectionHelper)
		{
			ISet<string> reservedDb = new HashSet<string>();
			await connectionHelper.PrepareAsync().ConfigureAwait(false);
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

		private static void QuoteTable(Table table, ICollection<string> reservedDb)
		{
			if (!table.IsQuoted && reservedDb.Contains(table.Name.ToLowerInvariant()))
			{
				table.Name = GetNhQuoted(table.Name);
			}
			foreach (var column in table.ColumnIterator)
			{
				if (!column.IsQuoted && reservedDb.Contains(column.Name.ToLowerInvariant()))
				{
					column.Name = GetNhQuoted(column.Name);
				}
			}
		}

		private static string GetNhQuoted(string name)
		{
			return "`" + name + "`";
		}
	}
}