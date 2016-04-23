using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NHibernate.Cache;
using NHibernate.Cache.Entry;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;

namespace NHibernate.Event.Default
{
	[Serializable]
	public class DefaultInitializeCollectionEventListener : IInitializeCollectionEventListener
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(DefaultInitializeCollectionEventListener));

		/// <summary> called by a collection that wants to initialize itself</summary>
		public virtual async Task OnInitializeCollection(InitializeCollectionEvent @event)
		{
			IPersistentCollection collection = @event.Collection;
			ISessionImplementor source = @event.Session;

			bool statsEnabled = source.Factory.Statistics.IsStatisticsEnabled;
			var stopWath = new Stopwatch();
			if (statsEnabled)
			{
				stopWath.Start();
			}

			CollectionEntry ce = source.PersistenceContext.GetCollectionEntry(collection);
			if (ce == null)
				throw new HibernateException("collection was evicted");
			if (!collection.WasInitialized)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("initializing collection " + MessageHelper.CollectionInfoString(ce.LoadedPersister, collection, ce.LoadedKey, source));
				}

				log.Debug("checking second-level cache");
				bool foundInCache = await InitializeCollectionFromCache(ce.LoadedKey, ce.LoadedPersister, collection, source).ConfigureAwait(false);

				if (foundInCache)
				{
					log.Debug("collection initialized from cache");
				}
				else
				{
					log.Debug("collection not cached");
					await ce.LoadedPersister.Initialize(ce.LoadedKey, source).ConfigureAwait(false);
					log.Debug("collection initialized");

					if (statsEnabled)
					{
						stopWath.Stop();
						source.Factory.StatisticsImplementor.FetchCollection(ce.LoadedPersister.Role, stopWath.Elapsed);
					}
				}
			}
		}

		/// <summary> Try to initialize a collection from the cache</summary>
		private async Task<bool> InitializeCollectionFromCache(object id, ICollectionPersister persister, IPersistentCollection collection, ISessionImplementor source)
		{

			if (!(source.EnabledFilters.Count == 0) && persister.IsAffectedByEnabledFilters(source))
			{
				log.Debug("disregarding cached version (if any) of collection due to enabled filters ");
				return false;
			}

			bool useCache = persister.HasCache && ((source.CacheMode & CacheMode.Get) == CacheMode.Get);

			if (!useCache)
			{
				return false;
			}
			else
			{
				ISessionFactoryImplementor factory = source.Factory;

				CacheKey ck = source.GenerateCacheKey(id, persister.KeyType, persister.Role);
				object ce = persister.Cache.Get(ck, source.Timestamp);

				if (factory.Statistics.IsStatisticsEnabled)
				{
					if (ce == null)
					{
						factory.StatisticsImplementor.SecondLevelCacheMiss(persister.Cache.RegionName);
					}
					else
					{
						factory.StatisticsImplementor.SecondLevelCacheHit(persister.Cache.RegionName);
					}
				}

				if (ce == null)
				{
					log.DebugFormat("Collection cache miss: {0}", ck);
				}
				else
				{
					log.DebugFormat("Collection cache hit: {0}", ck);
				}

				if (ce == null)
				{
					return false;
				}
				else
				{
					IPersistenceContext persistenceContext = source.PersistenceContext;

					CollectionCacheEntry cacheEntry = (CollectionCacheEntry)persister.CacheEntryStructure.Destructure(ce, factory);
					await cacheEntry.Assemble(collection, persister, persistenceContext.GetCollectionOwner(id, persister)).ConfigureAwait(false);

					persistenceContext.GetCollectionEntry(collection).PostInitialize(collection);
					return true;
				}
			}
		}
	}
}
