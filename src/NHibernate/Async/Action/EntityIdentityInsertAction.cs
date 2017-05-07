﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Diagnostics;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;

namespace NHibernate.Action
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
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

			bool veto = await (PreInsertAsync()).ConfigureAwait(false);

			// Don't need to lock the cache here, since if someone
			// else inserted the same pk first, the insert would fail

			if (!veto)
			{
				generatedId = await (persister.InsertAsync(state, instance, Session)).ConfigureAwait(false);
				if (persister.HasInsertGeneratedProperties)
				{
					await (persister.ProcessInsertGeneratedPropertiesAsync(generatedId, instance, state, Session)).ConfigureAwait(false);
				}
				//need to do that here rather than in the save event listener to let
				//the post insert events to have a id-filled entity when IDENTITY is used (EJB3)
				persister.SetIdentifier(instance, generatedId);
			}

			//TODO from H3.2 : this bit actually has to be called after all cascades!
			//      but since identity insert is called *synchronously*,
			//      instead of asynchronously as other actions, it isn't
			/*if ( persister.hasCache() && !persister.isCacheInvalidationRequired() ) {
			cacheEntry = new CacheEntry(object, persister, session);
			persister.getCache().insert(generatedId, cacheEntry);
			}*/

			await (PostInsertAsync()).ConfigureAwait(false);
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
					await (listener.OnPostInsertAsync(postEvent)).ConfigureAwait(false);
				}
			}
		}

		private async Task PostCommitInsertAsync()
		{
			IPostInsertEventListener[] postListeners = Session.Listeners.PostCommitInsertEventListeners;
			if (postListeners.Length > 0)
			{
				var postEvent = new PostInsertEvent(Instance, generatedId, state, Persister, (IEventSource) Session);
				foreach (IPostInsertEventListener listener in postListeners)
				{
					await (listener.OnPostInsertAsync(postEvent)).ConfigureAwait(false);
				}
			}
		}

		private async Task<bool> PreInsertAsync()
		{
			IPreInsertEventListener[] preListeners = Session.Listeners.PreInsertEventListeners;
			bool veto = false;
			if (preListeners.Length > 0)
			{
				var preEvent = new PreInsertEvent(Instance, null, state, Persister, (IEventSource) Session);
				foreach (IPreInsertEventListener listener in preListeners)
				{
					veto |= await (listener.OnPreInsertAsync(preEvent)).ConfigureAwait(false);
				}
			}
			return veto;
		}

		protected override Task AfterTransactionCompletionProcessImplAsync(bool success)
		{
			try
			{
				//TODO Make 100% certain that this is called before any subsequent ScheduledUpdate.afterTransactionCompletion()!!
				//TODO from H3.2: reenable if we also fix the above todo
				/*EntityPersister persister = getEntityPersister();
			if ( success && persister.hasCache() && !persister.isCacheInvalidationRequired() ) {
			persister.getCache().afterInsert( getGeneratedId(), cacheEntry );
			}*/
				if (success)
				{
					return PostCommitInsertAsync();
				}

				return Task.CompletedTask;
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}
	}
}
