#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;
using NHibernate.Util;

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
				await (persister.SetIdentifierAsync(instance, generatedId, Session.EntityMode));
			}

			//TODO from H3.2 : this bit actually has to be called after all cascades!
			//      but since identity insert is called *synchronously*,
			//      instead of asynchronously as other actions, it isn't
			/*if ( persister.hasCache() && !persister.isCacheInvalidationRequired() ) {
			cacheEntry = new CacheEntry(object, persister, session);
			persister.getCache().insert(generatedId, cacheEntry);
			}*/
			PostInsert();
			if (statsEnabled && !veto)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.InsertEntity(Persister.EntityName, stopwatch.Elapsed);
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

		protected override Task AfterTransactionCompletionProcessImplAsync(bool success)
		{
			try
			{
				AfterTransactionCompletionProcessImpl(success);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
