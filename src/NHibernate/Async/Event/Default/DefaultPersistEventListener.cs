#if NET_4_5
using System;
using System.Collections;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultPersistEventListener : AbstractSaveEventListener, IPersistEventListener
	{
		public virtual async Task OnPersistAsync(PersistEvent @event)
		{
			await (OnPersistAsync(@event, IdentityMap.Instantiate(10)));
		}

		public virtual async Task OnPersistAsync(PersistEvent @event, IDictionary createdAlready)
		{
			ISessionImplementor source = @event.Session;
			object obj = @event.Entity;
			object entity;
			if (obj.IsProxy())
			{
				ILazyInitializer li = ((INHibernateProxy)obj).HibernateLazyInitializer;
				if (li.IsUninitialized)
				{
					if (li.Session == source)
					{
						return; //NOTE EARLY EXIT!
					}
					else
					{
						throw new PersistentObjectException("uninitialized proxy passed to persist()");
					}
				}

				entity = await (li.GetImplementationAsync());
			}
			else
			{
				entity = obj;
			}

			EntityState entityState = await (GetEntityStateAsync(entity, @event.EntityName, source.PersistenceContext.GetEntry(entity), source));
			switch (entityState)
			{
				case EntityState.Persistent:
					await (EntityIsPersistentAsync(@event, createdAlready));
					break;
				case EntityState.Transient:
					await (EntityIsTransientAsync(@event, createdAlready));
					break;
				case EntityState.Detached:
					throw new PersistentObjectException("detached entity passed to persist: " + GetLoggableName(@event.EntityName, entity));
				default:
					throw new ObjectDeletedException("deleted instance passed to merge", null, GetLoggableName(@event.EntityName, entity));
			}
		}

		protected virtual async Task EntityIsPersistentAsync(PersistEvent @event, IDictionary createCache)
		{
			log.Debug("ignoring persistent instance");
			IEventSource source = @event.Session;
			//TODO: check that entry.getIdentifier().equals(requestedId)
			object entity = await (source.PersistenceContext.UnproxyAsync(@event.Entity));
			/* NH-2565: the UnProxy may return a "field interceptor proxy". When EntityName is null the session.GetEntityPersister will try to guess it.
			 * Instead change a session's method I'll try to guess the EntityName here.
			 * Because I'm using a session's method perhaps could be better if each session's method, which implementation forward to a method having the EntityName as parameter,
			 * use the BestGuessEntityName directly instead do "70 turns" before call it.
			*/
			if (@event.EntityName == null)
			{
				@event.EntityName = await (source.BestGuessEntityNameAsync(entity));
			}

			IEntityPersister persister = source.GetEntityPersister(@event.EntityName, entity);
			object tempObject;
			tempObject = createCache[entity];
			createCache[entity] = entity;
			if (tempObject == null)
			{
				await (CascadeBeforeSaveAsync(source, persister, entity, createCache));
				await (CascadeAfterSaveAsync(source, persister, entity, createCache));
			}
		}

		/// <summary> Handle the given create event. </summary>
		/// <param name = "event">The save event to be handled. </param>
		/// <param name = "createCache"></param>
		protected virtual async Task EntityIsTransientAsync(PersistEvent @event, IDictionary createCache)
		{
			log.Debug("saving transient instance");
			IEventSource source = @event.Session;
			object entity = await (source.PersistenceContext.UnproxyAsync(@event.Entity));
			object tempObject;
			tempObject = createCache[entity];
			createCache[entity] = entity;
			if (tempObject == null)
			{
				await (SaveWithGeneratedIdAsync(entity, @event.EntityName, createCache, source, false));
			}
		}
	}
}
#endif
