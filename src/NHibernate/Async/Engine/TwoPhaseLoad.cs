#if NET_4_5
using System.Diagnostics;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Event;
using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.Properties;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class TwoPhaseLoad
	{
		/// <summary>
		/// Perform the second step of 2-phase load. Fully initialize the entity instance.
		/// After processing a JDBC result set, we "resolve" all the associations
		/// between the entities which were instantiated and had their state
		/// "hydrated" into an array
		/// </summary>
		public static async Task InitializeEntityAsync(object entity, bool readOnly, ISessionImplementor session, PreLoadEvent preLoadEvent, PostLoadEvent postLoadEvent)
		{
			//TODO: Should this be an InitializeEntityEventListener??? (watch out for performance!)
			bool statsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
			var stopWath = new Stopwatch();
			if (statsEnabled)
			{
				stopWath.Start();
			}

			IPersistenceContext persistenceContext = session.PersistenceContext;
			EntityEntry entityEntry = persistenceContext.GetEntry(entity);
			if (entityEntry == null)
			{
				throw new AssertionFailure("possible non-threadsafe access to the session");
			}

			IEntityPersister persister = entityEntry.Persister;
			object id = entityEntry.Id;
			object[] hydratedState = entityEntry.LoadedState;
			if (log.IsDebugEnabled)
				log.Debug("resolving associations for " + MessageHelper.InfoString(persister, id, session.Factory));
			IType[] types = persister.PropertyTypes;
			for (int i = 0; i < hydratedState.Length; i++)
			{
				object value = hydratedState[i];
				if (!Equals(LazyPropertyInitializer.UnfetchedProperty, value) && !(Equals(BackrefPropertyAccessor.Unknown, value)))
				{
					hydratedState[i] = await (types[i].ResolveIdentifierAsync(value, session, entity));
				}
			}

			//Must occur after resolving identifiers!
			if (session.IsEventSource)
			{
				preLoadEvent.Entity = entity;
				preLoadEvent.State = hydratedState;
				preLoadEvent.Id = id;
				preLoadEvent.Persister = persister;
				IPreLoadEventListener[] listeners = session.Listeners.PreLoadEventListeners;
				for (int i = 0; i < listeners.Length; i++)
				{
					listeners[i].OnPreLoad(preLoadEvent);
				}
			}

			persister.SetPropertyValues(entity, hydratedState, session.EntityMode);
			ISessionFactoryImplementor factory = session.Factory;
			if (persister.HasCache && ((session.CacheMode & CacheMode.Put) == CacheMode.Put))
			{
				if (log.IsDebugEnabled)
					log.Debug("adding entity to second-level cache: " + MessageHelper.InfoString(persister, id, session.Factory));
				object version = Versioning.GetVersion(hydratedState, persister);
				CacheEntry entry = new CacheEntry(hydratedState, persister, entityEntry.LoadedWithLazyPropertiesUnfetched, version, session, entity);
				CacheKey cacheKey = session.GenerateCacheKey(id, persister.IdentifierType, persister.RootEntityName);
				bool put = persister.Cache.Put(cacheKey, persister.CacheEntryStructure.Structure(entry), session.Timestamp, version, persister.IsVersioned ? persister.VersionType.Comparator : null, UseMinimalPuts(session, entityEntry));
				if (put && factory.Statistics.IsStatisticsEnabled)
				{
					factory.StatisticsImplementor.SecondLevelCachePut(persister.Cache.RegionName);
				}
			}

			bool isReallyReadOnly = readOnly;
			if (!persister.IsMutable)
			{
				isReallyReadOnly = true;
			}
			else
			{
				object proxy = persistenceContext.GetProxy(entityEntry.EntityKey);
				if (proxy != null)
				{
					// there is already a proxy for this impl
					// only set the status to read-only if the proxy is read-only
					isReallyReadOnly = ((INHibernateProxy)proxy).HibernateLazyInitializer.ReadOnly;
				}
			}

			if (isReallyReadOnly)
			{
				//no need to take a snapshot - this is a
				//performance optimization, but not really
				//important, except for entities with huge
				//mutable property values
				persistenceContext.SetEntryStatus(entityEntry, Status.ReadOnly);
			}
			else
			{
				//take a snapshot
				TypeHelper.DeepCopy(hydratedState, persister.PropertyTypes, persister.PropertyUpdateability, hydratedState, session);
				persistenceContext.SetEntryStatus(entityEntry, Status.Loaded);
			}

			persister.AfterInitialize(entity, entityEntry.LoadedWithLazyPropertiesUnfetched, session);
			if (session.IsEventSource)
			{
				postLoadEvent.Entity = entity;
				postLoadEvent.Id = id;
				postLoadEvent.Persister = persister;
				IPostLoadEventListener[] listeners = session.Listeners.PostLoadEventListeners;
				for (int i = 0; i < listeners.Length; i++)
				{
					listeners[i].OnPostLoad(postLoadEvent);
				}
			}

			if (log.IsDebugEnabled)
				log.Debug("done materializing entity " + MessageHelper.InfoString(persister, id, session.Factory));
			if (statsEnabled)
			{
				stopWath.Stop();
				factory.StatisticsImplementor.LoadEntity(persister.EntityName, stopWath.Elapsed);
			}
		}
	}
}
#endif
