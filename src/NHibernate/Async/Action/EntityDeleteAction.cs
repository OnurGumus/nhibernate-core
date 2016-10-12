﻿#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Cache;
using NHibernate.Cache.Access;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class EntityDeleteAction : EntityAction
	{
		public override async Task ExecuteAsync()
		{
			object id = Id;
			IEntityPersister persister = Persister;
			ISessionImplementor session = Session;
			object instance = Instance;
			bool statsEnabled = Session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			bool veto = await (PreDeleteAsync());
			object tmpVersion = version;
			if (persister.IsVersionPropertyGenerated)
			{
				// we need to grab the version value from the entity, otherwise
				// we have issues with generated-version entities that may have
				// multiple actions queued during the same flush
				tmpVersion = persister.GetVersion(instance, session.EntityMode);
			}

			CacheKey ck;
			if (persister.HasCache)
			{
				ck = session.GenerateCacheKey(id, persister.IdentifierType, persister.RootEntityName);
				sLock = persister.Cache.Lock(ck, version);
			}
			else
			{
				ck = null;
			}

			if (!isCascadeDeleteEnabled && !veto)
			{
				await (persister.DeleteAsync(id, tmpVersion, instance, session));
			}

			//postDelete:
			// After actually deleting a row, record the fact that the instance no longer 
			// exists on the database (needed for identity-column key generation), and
			// remove it from the session cache
			IPersistenceContext persistenceContext = session.PersistenceContext;
			EntityEntry entry = persistenceContext.RemoveEntry(instance);
			if (entry == null)
			{
				throw new AssertionFailure("Possible nonthreadsafe access to session");
			}

			entry.PostDelete();
			EntityKey key = session.GenerateEntityKey(entry.Id, entry.Persister);
			persistenceContext.RemoveEntity(key);
			persistenceContext.RemoveProxy(key);
			if (persister.HasCache)
				persister.Cache.Evict(ck);
			await (PostDeleteAsync());
			if (statsEnabled && !veto)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.DeleteEntity(Persister.EntityName, stopwatch.Elapsed);
			}
		}

		private async Task PostDeleteAsync()
		{
			IPostDeleteEventListener[] postListeners = Session.Listeners.PostDeleteEventListeners;
			if (postListeners.Length > 0)
			{
				PostDeleteEvent postEvent = new PostDeleteEvent(Instance, Id, state, Persister, (IEventSource)Session);
				foreach (IPostDeleteEventListener listener in postListeners)
				{
					await (listener.OnPostDeleteAsync(postEvent));
				}
			}
		}

		private async Task<bool> PreDeleteAsync()
		{
			IPreDeleteEventListener[] preListeners = Session.Listeners.PreDeleteEventListeners;
			bool veto = false;
			if (preListeners.Length > 0)
			{
				var preEvent = new PreDeleteEvent(Instance, Id, state, Persister, (IEventSource)Session);
				foreach (IPreDeleteEventListener listener in preListeners)
				{
					veto |= await (listener.OnPreDeleteAsync(preEvent));
				}
			}

			return veto;
		}

		protected override async Task AfterTransactionCompletionProcessImplAsync(bool success)
		{
			if (Persister.HasCache)
			{
				CacheKey ck = Session.GenerateCacheKey(Id, Persister.IdentifierType, Persister.RootEntityName);
				Persister.Cache.Release(ck, sLock);
			}

			if (success)
			{
				await (PostCommitDeleteAsync());
			}
		}

		private async Task PostCommitDeleteAsync()
		{
			IPostDeleteEventListener[] postListeners = Session.Listeners.PostCommitDeleteEventListeners;
			if (postListeners.Length > 0)
			{
				PostDeleteEvent postEvent = new PostDeleteEvent(Instance, Id, state, Persister, (IEventSource)Session);
				foreach (IPostDeleteEventListener listener in postListeners)
				{
					await (listener.OnPostDeleteAsync(postEvent));
				}
			}
		}
	}
}
#endif
