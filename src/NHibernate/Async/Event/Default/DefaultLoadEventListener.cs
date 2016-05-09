using System;
using System.Diagnostics;
using System.Text;
using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Cache.Entry;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultLoadEventListener : AbstractLockUpgradeEventListener, ILoadEventListener
	{
		protected virtual async Task<object> LoadFromDatasourceAsync(LoadEvent @event, IEntityPersister persister, EntityKey keyToLoad, LoadType options)
		{
			ISessionImplementor source = @event.Session;
			bool statsEnabled = source.Factory.Statistics.IsStatisticsEnabled;
			var stopWath = new Stopwatch();
			if (statsEnabled)
			{
				stopWath.Start();
			}

			object entity = await (persister.LoadAsync(@event.EntityId, @event.InstanceToLoad, @event.LockMode, source));
			if (@event.IsAssociationFetch && statsEnabled)
			{
				stopWath.Stop();
				source.Factory.StatisticsImplementor.FetchEntity(@event.EntityClassName, stopWath.Elapsed);
			}

			return entity;
		}

		protected virtual async Task<object> DoLoadAsync(LoadEvent @event, IEntityPersister persister, EntityKey keyToLoad, LoadType options)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("attempting to resolve: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory));
			}

			object entity = await (LoadFromSessionCacheAsync(@event, keyToLoad, options));
			if (entity == RemovedEntityMarker)
			{
				log.Debug("load request found matching entity in context, but it is scheduled for removal; returning null");
				return null;
			}

			if (entity == InconsistentRTNClassMarker)
			{
				log.Debug("load request found matching entity in context, but the matched entity was of an inconsistent return type; returning null");
				return null;
			}

			if (entity != null)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("resolved object in session cache: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory));
				}

				return entity;
			}

			entity = await (LoadFromSecondLevelCacheAsync(@event, persister, options));
			if (entity != null)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("resolved object in second-level cache: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory));
				}

				return entity;
			}

			if (log.IsDebugEnabled)
			{
				log.Debug("object not resolved in any cache: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory));
			}

			return await (LoadFromDatasourceAsync(@event, persister, keyToLoad, options));
		}

		protected virtual async Task<object> LoadAsync(LoadEvent @event, IEntityPersister persister, EntityKey keyToLoad, LoadType options)
		{
			if (@event.InstanceToLoad != null)
			{
				if (@event.Session.PersistenceContext.GetEntry(@event.InstanceToLoad) != null)
				{
					throw new PersistentObjectException("attempted to load into an instance that was already associated with the session: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory));
				}

				await (persister.SetIdentifierAsync(@event.InstanceToLoad, @event.EntityId, @event.Session.EntityMode));
			}

			object entity = await (DoLoadAsync(@event, persister, keyToLoad, options));
			bool isOptionalInstance = @event.InstanceToLoad != null;
			if (!options.IsAllowNulls || isOptionalInstance)
			{
				if (entity == null)
				{
					@event.Session.Factory.EntityNotFoundDelegate.HandleEntityNotFound(@event.EntityClassName, @event.EntityId);
				}
			}

			if (isOptionalInstance && entity != @event.InstanceToLoad)
			{
				throw new NonUniqueObjectException(@event.EntityId, @event.EntityClassName);
			}

			return entity;
		}

		protected virtual async Task<object> LockAndLoadAsync(LoadEvent @event, IEntityPersister persister, EntityKey keyToLoad, LoadType options, ISessionImplementor source)
		{
			ISoftLock sLock = null;
			CacheKey ck;
			if (persister.HasCache)
			{
				ck = source.GenerateCacheKey(@event.EntityId, persister.IdentifierType, persister.RootEntityName);
				sLock = persister.Cache.Lock(ck, null);
			}
			else
			{
				ck = null;
			}

			object entity;
			try
			{
				entity = await (LoadAsync(@event, persister, keyToLoad, options));
			}
			finally
			{
				if (persister.HasCache)
				{
					persister.Cache.Release(ck, sLock);
				}
			}

			object proxy = @event.Session.PersistenceContext.ProxyFor(persister, keyToLoad, entity);
			return proxy;
		}

		public virtual async Task OnLoadAsync(LoadEvent @event, LoadType loadType)
		{
			ISessionImplementor source = @event.Session;
			IEntityPersister persister;
			if (@event.InstanceToLoad != null)
			{
				persister = source.GetEntityPersister(null, @event.InstanceToLoad); //the load() which takes an entity does not pass an entityName
				@event.EntityClassName = @event.InstanceToLoad.GetType().FullName;
			}
			else
			{
				persister = GetEntityPersister(source.Factory, @event.EntityClassName);
			}

			if (persister == null)
			{
				var message = new StringBuilder(512);
				message.AppendLine(string.Format("Unable to locate persister for the entity named '{0}'.", @event.EntityClassName));
				message.AppendLine("The persister define the persistence strategy for an entity.");
				message.AppendLine("Possible causes:");
				message.AppendLine(string.Format(" - The mapping for '{0}' was not added to the NHibernate configuration.", @event.EntityClassName));
				throw new HibernateException(message.ToString());
			}

			if (persister.IdentifierType.IsComponentType)
			{
			// skip this check for composite-ids relating to dom4j entity-mode;
			// alternatively, we could add a check to make sure the incoming id value is
			// an instance of Element...
			}
			else
			{
				System.Type idClass = persister.IdentifierType.ReturnedClass;
				if (idClass != null && !idClass.IsInstanceOfType(@event.EntityId))
				{
					throw new TypeMismatchException("Provided id of the wrong type. Expected: " + idClass + ", got " + @event.EntityId.GetType());
				}
			}

			EntityKey keyToLoad = source.GenerateEntityKey(@event.EntityId, persister);
			try
			{
				if (loadType.IsNakedEntityReturned)
				{
					//do not return a proxy!
					//(this option indicates we are initializing a proxy)
					@event.Result = await (LoadAsync(@event, persister, keyToLoad, loadType));
				}
				else
				{
					//return a proxy if appropriate
					if (@event.LockMode == LockMode.None)
					{
						@event.Result = await (ProxyOrLoadAsync(@event, persister, keyToLoad, loadType));
					}
					else
					{
						@event.Result = await (LockAndLoadAsync(@event, persister, keyToLoad, loadType, source));
					}
				}
			}
			catch (HibernateException e)
			{
				log.Info("Error performing load command", e);
				throw;
			}
		}

		private async Task<object> AssembleCacheEntryAsync(CacheEntry entry, object id, IEntityPersister persister, LoadEvent @event)
		{
			object optionalObject = @event.InstanceToLoad;
			IEventSource session = @event.Session;
			ISessionFactoryImplementor factory = session.Factory;
			if (log.IsDebugEnabled)
			{
				log.Debug("assembling entity from second-level cache: " + MessageHelper.InfoString(persister, id, factory));
			}

			IEntityPersister subclassPersister = factory.GetEntityPersister(entry.Subclass);
			object result = optionalObject ?? await (session.InstantiateAsync(subclassPersister, id));
			// make it circular-reference safe
			EntityKey entityKey = session.GenerateEntityKey(id, subclassPersister);
			TwoPhaseLoad.AddUninitializedCachedEntity(entityKey, result, subclassPersister, LockMode.None, entry.AreLazyPropertiesUnfetched, entry.Version, session);
			IType[] types = subclassPersister.PropertyTypes;
			object[] values = await (entry.AssembleAsync(result, id, subclassPersister, session.Interceptor, session)); // intializes result by side-effect
			await (TypeHelper.DeepCopyAsync(values, types, subclassPersister.PropertyUpdateability, values, session));
			object version = Versioning.GetVersion(values, subclassPersister);
			if (log.IsDebugEnabled)
			{
				log.Debug("Cached Version: " + version);
			}

			IPersistenceContext persistenceContext = session.PersistenceContext;
			bool isReadOnly = session.DefaultReadOnly;
			if (persister.IsMutable)
			{
				object proxy = persistenceContext.GetProxy(entityKey);
				if (proxy != null)
				{
					// this is already a proxy for this impl
					// only set the status to read-only if the proxy is read-only
					isReadOnly = ((INHibernateProxy)proxy).HibernateLazyInitializer.ReadOnly;
				}
			}
			else
				isReadOnly = true;
			persistenceContext.AddEntry(result, isReadOnly ? Status.ReadOnly : Status.Loaded, values, null, id, version, LockMode.None, true, subclassPersister, false, entry.AreLazyPropertiesUnfetched);
			subclassPersister.AfterInitialize(result, entry.AreLazyPropertiesUnfetched, session);
			await (persistenceContext.InitializeNonLazyCollectionsAsync());
			// upgrade the lock if necessary:
			//lock(result, lockMode);
			//PostLoad is needed for EJB3
			//TODO: reuse the PostLoadEvent...
			PostLoadEvent postLoadEvent = new PostLoadEvent(session);
			postLoadEvent.Entity = result;
			postLoadEvent.Id = id;
			postLoadEvent.Persister = persister;
			IPostLoadEventListener[] listeners = session.Listeners.PostLoadEventListeners;
			for (int i = 0; i < listeners.Length; i++)
			{
				listeners[i].OnPostLoad(postLoadEvent);
			}

			return result;
		}

		protected virtual async Task<object> LoadFromSecondLevelCacheAsync(LoadEvent @event, IEntityPersister persister, LoadType options)
		{
			ISessionImplementor source = @event.Session;
			bool useCache = persister.HasCache && ((source.CacheMode & CacheMode.Get) == CacheMode.Get) && @event.LockMode.LessThan(LockMode.Read);
			if (useCache)
			{
				ISessionFactoryImplementor factory = source.Factory;
				CacheKey ck = source.GenerateCacheKey(@event.EntityId, persister.IdentifierType, persister.RootEntityName);
				object ce = persister.Cache.Get(ck, source.Timestamp);
				if (factory.Statistics.IsStatisticsEnabled)
				{
					if (ce == null)
					{
						factory.StatisticsImplementor.SecondLevelCacheMiss(persister.Cache.RegionName);
						log.DebugFormat("Entity cache miss: {0}", ck);
					}
					else
					{
						factory.StatisticsImplementor.SecondLevelCacheHit(persister.Cache.RegionName);
						log.DebugFormat("Entity cache hit: {0}", ck);
					}
				}

				if (ce != null)
				{
					CacheEntry entry = (CacheEntry)persister.CacheEntryStructure.Destructure(ce, factory);
					// Entity was found in second-level cache...
					// NH: Different behavior (take a look to options.ExactPersister (NH-295))
					if (!options.ExactPersister || persister.EntityMetamodel.SubclassEntityNames.Contains(entry.Subclass))
					{
						return await (AssembleCacheEntryAsync(entry, @event.EntityId, persister, @event));
					}
				}
			}

			return null;
		}

		protected virtual async Task<object> LoadFromSessionCacheAsync(LoadEvent @event, EntityKey keyToLoad, LoadType options)
		{
			ISessionImplementor session = @event.Session;
			object old = await (session.GetEntityUsingInterceptorAsync(keyToLoad));
			if (old != null)
			{
				// this object was already loaded
				EntityEntry oldEntry = session.PersistenceContext.GetEntry(old);
				if (options.IsCheckDeleted)
				{
					Status status = oldEntry.Status;
					if (status == Status.Deleted || status == Status.Gone)
					{
						return RemovedEntityMarker;
					}
				}

				if (options.IsAllowNulls)
				{
					IEntityPersister persister = GetEntityPersister(@event.Session.Factory, @event.EntityClassName);
					if (!persister.IsInstance(old, @event.Session.EntityMode))
					{
						return InconsistentRTNClassMarker;
					}
				}

				await (UpgradeLockAsync(old, oldEntry, @event.LockMode, session));
			}

			return old;
		}

		protected virtual async Task<object> ProxyOrLoadAsync(LoadEvent @event, IEntityPersister persister, EntityKey keyToLoad, LoadType options)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("loading entity: " + MessageHelper.InfoString(persister, @event.EntityId, @event.Session.Factory));
			}

			if (!persister.HasProxy)
			{
				// this class has no proxies (so do a shortcut)
				return await (LoadAsync(@event, persister, keyToLoad, options));
			}
			else
			{
				IPersistenceContext persistenceContext = @event.Session.PersistenceContext;
				// look for a proxy
				object proxy = persistenceContext.GetProxy(keyToLoad);
				if (proxy != null)
				{
					return await (ReturnNarrowedProxyAsync(@event, persister, keyToLoad, options, persistenceContext, proxy));
				}
				else
				{
					if (options.IsAllowProxyCreation)
					{
						return CreateProxyIfNecessary(@event, persister, keyToLoad, options, persistenceContext);
					}
					else
					{
						// return a newly loaded object
						return await (LoadAsync(@event, persister, keyToLoad, options));
					}
				}
			}
		}

		private async Task<object> ReturnNarrowedProxyAsync(LoadEvent @event, IEntityPersister persister, EntityKey keyToLoad, LoadType options, IPersistenceContext persistenceContext, object proxy)
		{
			log.Debug("entity proxy found in session cache");
			var castedProxy = (INHibernateProxy)proxy;
			ILazyInitializer li = castedProxy.HibernateLazyInitializer;
			if (li.Unwrap)
			{
				return await (li.GetImplementationAsync());
			}

			object impl = null;
			if (!options.IsAllowProxyCreation)
			{
				impl = await (LoadAsync(@event, persister, keyToLoad, options));
				// NH Different behavior : NH-1252
				if (impl == null && !options.IsAllowNulls)
				{
					@event.Session.Factory.EntityNotFoundDelegate.HandleEntityNotFound(persister.EntityName, keyToLoad.Identifier);
				}
			}

			if (impl == null && !options.IsAllowProxyCreation && options.ExactPersister)
			{
				// NH Different behavior : NH-1252
				return null;
			}

			return persistenceContext.NarrowProxy(castedProxy, persister, keyToLoad, impl);
		}
	}
}