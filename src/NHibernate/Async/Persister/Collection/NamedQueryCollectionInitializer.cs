#if NET_4_5
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader.Collection;
using System.Threading.Tasks;

namespace NHibernate.Persister.Collection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NamedQueryCollectionInitializer : ICollectionInitializer
	{
		public async Task InitializeAsync(object key, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug(string.Format("initializing collection: {0} using named query: {1}", persister.Role, queryName));
			}

			//TODO: is there a more elegant way than downcasting?
			AbstractQueryImpl query = (AbstractQueryImpl)session.GetNamedSQLQuery(queryName);
			if (query.NamedParameters.Length > 0)
			{
				query.SetParameter(query.NamedParameters[0], key, persister.KeyType);
			}
			else
			{
				query.SetParameter(0, key, persister.KeyType);
			}

			await (query.SetCollectionKey(key).SetFlushMode(FlushMode.Never).ListAsync());
		}
	}
}
#endif
