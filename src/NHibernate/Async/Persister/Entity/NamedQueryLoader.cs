#if NET_4_5
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Loader.Entity;
using System.Threading.Tasks;

namespace NHibernate.Persister.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NamedQueryLoader : IUniqueEntityLoader
	{
		public async Task<object> LoadAsync(object id, object optionalObject, ISessionImplementor session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug(string.Format("loading entity: {0} using named query: {1}", persister.EntityName, queryName));
			}

			AbstractQueryImpl query = (AbstractQueryImpl)session.GetNamedQuery(queryName);
			if (query.HasNamedParameters)
			{
				query.SetParameter(query.NamedParameters[0], id, persister.IdentifierType);
			}
			else
			{
				query.SetParameter(0, id, persister.IdentifierType);
			}

			query.SetOptionalId(id);
			query.SetOptionalEntityName(persister.EntityName);
			query.SetOptionalObject(optionalObject);
			query.SetFlushMode(FlushMode.Never);
			await (query.ListAsync());
			// now look up the object we are really interested in!
			// (this lets us correctly handle proxies and multi-row
			// or multi-column queries)
			return session.PersistenceContext.GetEntity(session.GenerateEntityKey(id, persister));
		}
	}
}
#endif
