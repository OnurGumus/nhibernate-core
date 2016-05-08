using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Id;
using NHibernate.Loader.Criteria;
using NHibernate.Loader.Custom;
using NHibernate.Loader.Custom.Sql;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatelessSessionImpl : AbstractSessionImpl, IStatelessSession
	{
		public async Task<object> InsertAsync(string entityName, object entity)
		{
			using (new SessionIdLoggingContext(SessionId))
			{
				CheckAndUpdateSessionStatus();
				IEntityPersister persister = GetEntityPersister(entityName, entity);
				object id = await (persister.IdentifierGenerator.GenerateAsync(this, entity));
				object[] state = persister.GetPropertyValues(entity, EntityMode.Poco);
				if (persister.IsVersioned)
				{
					object versionValue = state[persister.VersionProperty];
					bool substitute = Versioning.SeedVersion(state, persister.VersionProperty, persister.VersionType, persister.IsUnsavedVersion(versionValue), this);
					if (substitute)
					{
						persister.SetPropertyValues(entity, state, EntityMode.Poco);
					}
				}

				if (id == IdentifierGeneratorFactory.PostInsertIndicator)
				{
					id = persister.Insert(state, entity, this);
				}
				else
				{
					persister.Insert(id, state, entity, this);
				}

				persister.SetIdentifier(entity, id, EntityMode.Poco);
				return id;
			}
		}
	}
}