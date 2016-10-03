#if NET_4_5
using System;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultEvictEventListener : IEvictEventListener
	{
		public virtual async Task OnEvictAsync(EvictEvent @event)
		{
			IEventSource source = @event.Session;
			object obj = @event.Entity;
			IPersistenceContext persistenceContext = source.PersistenceContext;
			if (obj.IsProxy())
			{
				ILazyInitializer li = ((INHibernateProxy)obj).HibernateLazyInitializer;
				object id = li.Identifier;
				IEntityPersister persister = source.Factory.GetEntityPersister(li.EntityName);
				if (id == null)
				{
					throw new ArgumentException("null identifier");
				}

				EntityKey key = source.GenerateEntityKey(id, persister);
				persistenceContext.RemoveProxy(key);
				if (!li.IsUninitialized)
				{
					object entity = persistenceContext.RemoveEntity(key);
					if (entity != null)
					{
						EntityEntry e = @event.Session.PersistenceContext.RemoveEntry(entity);
						await (DoEvictAsync(entity, key, e.Persister, @event.Session));
					}
				}

				li.UnsetSession();
			}
			else
			{
				EntityEntry e = persistenceContext.RemoveEntry(obj);
				if (e != null)
				{
					persistenceContext.RemoveEntity(e.EntityKey);
					await (DoEvictAsync(obj, e.EntityKey, e.Persister, source));
				}
			}
		}

		protected virtual async Task DoEvictAsync(object obj, EntityKey key, IEntityPersister persister, IEventSource session)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("evicting " + MessageHelper.InfoString(persister));
			}

			// remove all collections for the entity from the session-level cache
			if (persister.HasCollections)
			{
				await (new EvictVisitor(session).ProcessAsync(obj, persister));
			}

			await (new Cascade(CascadingAction.Evict, CascadePoint.AfterEvict, session).CascadeOnAsync(persister, obj));
		}
	}
}
#endif
