#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class EntityIdentityInsertAction : EntityAction
	{
		public override async Task ExecuteAsync()
		{
			IEntityPersister persister = Persister;
			object instance = Instance;
			bool statsEnabled = Session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			bool veto = await (PreInsertAsync());
			// Don't need to lock the cache here, since if someone
			// else inserted the same pk first, the insert would fail
			if (!veto)
			{
				generatedId = await (persister.InsertAsync(state, instance, Session));
				if (persister.HasInsertGeneratedProperties)
				{
					await (persister.ProcessInsertGeneratedPropertiesAsync(generatedId, instance, state, Session));
				}

				//need to do that here rather than in the save event listener to let
				//the post insert events to have a id-filled entity when IDENTITY is used (EJB3)
				persister.SetIdentifier(instance, generatedId, Session.EntityMode);
			}

			await (PostInsertAsync());
			if (statsEnabled && !veto)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.InsertEntity(Persister.EntityName, stopwatch.Elapsed);
			}
		}

		private async Task PostInsertAsync()
		{
			if (isDelayed)
			{
				Session.PersistenceContext.ReplaceDelayedEntityIdentityInsertKeys(delayedEntityKey, generatedId);
			}

			IPostInsertEventListener[] postListeners = Session.Listeners.PostInsertEventListeners;
			if (postListeners.Length > 0)
			{
				PostInsertEvent postEvent = new PostInsertEvent(Instance, generatedId, state, Persister, (IEventSource)Session);
				foreach (IPostInsertEventListener listener in postListeners)
				{
					await (listener.OnPostInsertAsync(postEvent));
				}
			}
		}

		private async Task PostCommitInsertAsync()
		{
			IPostInsertEventListener[] postListeners = Session.Listeners.PostCommitInsertEventListeners;
			if (postListeners.Length > 0)
			{
				var postEvent = new PostInsertEvent(Instance, generatedId, state, Persister, (IEventSource)Session);
				foreach (IPostInsertEventListener listener in postListeners)
				{
					await (listener.OnPostInsertAsync(postEvent));
				}
			}
		}

		private async Task<bool> PreInsertAsync()
		{
			IPreInsertEventListener[] preListeners = Session.Listeners.PreInsertEventListeners;
			bool veto = false;
			if (preListeners.Length > 0)
			{
				var preEvent = new PreInsertEvent(Instance, null, state, Persister, (IEventSource)Session);
				foreach (IPreInsertEventListener listener in preListeners)
				{
					veto |= await (listener.OnPreInsertAsync(preEvent));
				}
			}

			return veto;
		}

		protected override async Task AfterTransactionCompletionProcessImplAsync(bool success)
		{
			//TODO Make 100% certain that this is called before any subsequent ScheduledUpdate.afterTransactionCompletion()!!
			//TODO from H3.2: reenable if we also fix the above todo
			/*EntityPersister persister = getEntityPersister();
			if ( success && persister.hasCache() && !persister.isCacheInvalidationRequired() ) {
			persister.getCache().afterInsert( getGeneratedId(), cacheEntry );
			}*/
			if (success)
			{
				await (PostCommitInsertAsync());
			}
		}
	}
}
#endif
