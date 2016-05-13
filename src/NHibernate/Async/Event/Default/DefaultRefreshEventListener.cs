using System;
using System.Collections;
using NHibernate.Cache;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultRefreshEventListener : IRefreshEventListener
	{
		public virtual async Task OnRefreshAsync(RefreshEvent @event)
		{
			await (OnRefreshAsync(@event, IdentityMap.Instantiate(10)));
		}

		public virtual async Task OnRefreshAsync(RefreshEvent @event, IDictionary refreshedAlready)
		{
			IEventSource source = @event.Session;
			bool isTransient = !await (source.ContainsAsync(@event.Entity));
			if (source.PersistenceContext.ReassociateIfUninitializedProxy(@event.Entity))
			{
				if (isTransient)
					await (source.SetReadOnlyAsync(@event.Entity, source.DefaultReadOnly));
				return;
			}

			object obj = await (source.PersistenceContext.UnproxyAndReassociateAsync(@event.Entity));
			if (refreshedAlready.Contains(obj))
			{
				log.Debug("already refreshed");
				return;
			}

			EntityEntry e = source.PersistenceContext.GetEntry(obj);
			IEntityPersister persister;
			object id;
			if (e == null)
			{
				persister = source.GetEntityPersister(null, obj); //refresh() does not pass an entityName
				id = await (persister.GetIdentifierAsync(obj, source.EntityMode));
				if (log.IsDebugEnabled)
				{
					log.Debug("refreshing transient " + MessageHelper.InfoString(persister, id, source.Factory));
				}

				EntityKey key = source.GenerateEntityKey(id, persister);
				if (source.PersistenceContext.GetEntry(key) != null)
				{
					throw new PersistentObjectException("attempted to refresh transient instance when persistent instance was already associated with the Session: " + MessageHelper.InfoString(persister, id, source.Factory));
				}
			}
			else
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("refreshing " + MessageHelper.InfoString(e.Persister, e.Id, source.Factory));
				}

				if (!e.ExistsInDatabase)
				{
					throw new HibernateException("this instance does not yet exist as a row in the database");
				}

				persister = e.Persister;
				id = e.Id;
			}

			// cascade the refresh prior to refreshing this entity
			refreshedAlready[obj] = obj;
			await (new Cascade(CascadingAction.Refresh, CascadePoint.BeforeRefresh, source).CascadeOnAsync(persister, obj, refreshedAlready));
			if (e != null)
			{
				EntityKey key = source.GenerateEntityKey(id, persister);
				source.PersistenceContext.RemoveEntity(key);
				if (persister.HasCollections)
					await (new EvictVisitor(source).ProcessAsync(obj, persister));
			}

			if (persister.HasCache)
			{
				CacheKey ck = source.GenerateCacheKey(id, persister.IdentifierType, persister.RootEntityName);
				persister.Cache.Remove(ck);
			}

			EvictCachedCollections(persister, id, source.Factory);
			// NH Different behavior : NH-1601
			// At this point the entity need the real refresh, all elementes of collections are Refreshed,
			// the collection state was evicted, but the PersistentCollection (in the entity state)
			// is associated with a possible previous session.
			await (new WrapVisitor(source).ProcessAsync(obj, persister));
			string previousFetchProfile = source.FetchProfile;
			source.FetchProfile = "refresh";
			object result = await (persister.LoadAsync(id, obj, @event.LockMode, source));
			if (result != null)
				if (!persister.IsMutable)
					await (source.SetReadOnlyAsync(result, true));
				else
					await (source.SetReadOnlyAsync(result, (e == null ? source.DefaultReadOnly : e.IsReadOnly)));
			source.FetchProfile = previousFetchProfile;
			// NH Different behavior : we are ignoring transient entities without throw any kind of exception
			// because a transient entity is "self refreshed"
			if (!await (ForeignKeys.IsTransientAsync(persister.EntityName, obj, result == null, @event.Session)))
				UnresolvableObjectException.ThrowIfNull(result, id, persister.EntityName);
		}
	}
}