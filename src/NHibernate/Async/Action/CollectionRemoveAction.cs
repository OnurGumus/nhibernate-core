#if NET_4_5
using System;
using System.Diagnostics;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Collection;
using System.Threading.Tasks;

namespace NHibernate.Action
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class CollectionRemoveAction : CollectionAction
	{
		public override async Task ExecuteAsync()
		{
			bool statsEnabled = Session.Factory.Statistics.IsStatisticsEnabled;
			Stopwatch stopwatch = null;
			if (statsEnabled)
			{
				stopwatch = Stopwatch.StartNew();
			}

			await (PreRemoveAsync());
			if (!emptySnapshot)
			{
				await (Persister.RemoveAsync(Key, Session));
			}

			IPersistentCollection collection = Collection;
			if (collection != null)
			{
				Session.PersistenceContext.GetCollectionEntry(collection).AfterAction(collection);
			}

			Evict();
			await (PostRemoveAsync());
			if (statsEnabled)
			{
				stopwatch.Stop();
				Session.Factory.StatisticsImplementor.RemoveCollection(Persister.Role, stopwatch.Elapsed);
			}
		}

		private async Task PreRemoveAsync()
		{
			IPreCollectionRemoveEventListener[] preListeners = Session.Listeners.PreCollectionRemoveEventListeners;
			if (preListeners.Length > 0)
			{
				PreCollectionRemoveEvent preEvent = new PreCollectionRemoveEvent(Persister, Collection, (IEventSource)Session, affectedOwner);
				for (int i = 0; i < preListeners.Length; i++)
				{
					await (preListeners[i].OnPreRemoveCollectionAsync(preEvent));
				}
			}
		}

		private async Task PostRemoveAsync()
		{
			IPostCollectionRemoveEventListener[] postListeners = Session.Listeners.PostCollectionRemoveEventListeners;
			if (postListeners.Length > 0)
			{
				PostCollectionRemoveEvent postEvent = new PostCollectionRemoveEvent(Persister, Collection, (IEventSource)Session, affectedOwner);
				for (int i = 0; i < postListeners.Length; i++)
				{
					await (postListeners[i].OnPostRemoveCollectionAsync(postEvent));
				}
			}
		}
	}
}
#endif
