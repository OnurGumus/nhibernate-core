using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Impl;
using NHibernate.Util;

namespace NHibernate.Test.Events.Collections
{
	public class CollectionListeners
	{
		private readonly IList<AbstractCollectionEvent> events = new List<AbstractCollectionEvent>();

		private readonly InitializeCollectionListener initializeCollectionListener;
		private readonly List<IListener> listenersCalled = new List<IListener>();
		private readonly PostCollectionRecreateListener postCollectionRecreateListener;
		private readonly PostCollectionRemoveListener postCollectionRemoveListener;
		private readonly PostCollectionUpdateListener postCollectionUpdateListener;
		private readonly PreCollectionRecreateListener preCollectionRecreateListener;
		private readonly PreCollectionRemoveListener preCollectionRemoveListener;
		private readonly PreCollectionUpdateListener preCollectionUpdateListener;

		public CollectionListeners(ISessionFactory sf)
		{
			preCollectionRecreateListener = new PreCollectionRecreateListener(this);
			initializeCollectionListener = new InitializeCollectionListener(this);
			preCollectionRemoveListener = new PreCollectionRemoveListener(this);
			preCollectionUpdateListener = new PreCollectionUpdateListener(this);
			postCollectionRecreateListener = new PostCollectionRecreateListener(this);
			postCollectionRemoveListener = new PostCollectionRemoveListener(this);
			postCollectionUpdateListener = new PostCollectionUpdateListener(this);
			SessionFactoryImpl impl = (SessionFactoryImpl) sf;
			impl.EventListeners.InitializeCollectionEventListeners = new IInitializeCollectionEventListener[]
			                                                         	{initializeCollectionListener};

			impl.EventListeners.PreCollectionRecreateEventListeners = new IPreCollectionRecreateEventListener[]
			                                                          	{preCollectionRecreateListener};
			impl.EventListeners.PostCollectionRecreateEventListeners = new IPostCollectionRecreateEventListener[]
			                                                           	{postCollectionRecreateListener};
			impl.EventListeners.PreCollectionRemoveEventListeners = new IPreCollectionRemoveEventListener[]
			                                                        	{preCollectionRemoveListener};
			impl.EventListeners.PostCollectionRemoveEventListeners = new IPostCollectionRemoveEventListener[]
			                                                         	{postCollectionRemoveListener};
			impl.EventListeners.PreCollectionUpdateEventListeners = new IPreCollectionUpdateEventListener[]
			                                                        	{preCollectionUpdateListener};
			impl.EventListeners.PostCollectionUpdateEventListeners = new IPostCollectionUpdateEventListener[]
			                                                         	{postCollectionUpdateListener};
		}

		public IList ListenersCalled
		{
			get { return listenersCalled; }
		}

		public IList<AbstractCollectionEvent> Events
		{
			get { return events; }
		}

		public void AddEvent(AbstractCollectionEvent @event, IListener listener)
		{
			listenersCalled.Add(listener);
			events.Add(@event);
		}

		public void Clear()
		{
			listenersCalled.Clear();
			events.Clear();
		}

		public InitializeCollectionListener InitializeCollection
		{
			get { return initializeCollectionListener; }
		}

		public PostCollectionRecreateListener PostCollectionRecreate
		{
			get { return postCollectionRecreateListener; }
		}

		public PostCollectionRemoveListener PostCollectionRemove
		{
			get { return postCollectionRemoveListener; }
		}

		public PostCollectionUpdateListener PostCollectionUpdate
		{
			get { return postCollectionUpdateListener; }
		}

		public PreCollectionRecreateListener PreCollectionRecreate
		{
			get { return preCollectionRecreateListener; }
		}

		public PreCollectionRemoveListener PreCollectionRemove
		{
			get { return preCollectionRemoveListener; }
		}

		public PreCollectionUpdateListener PreCollectionUpdate
		{
			get { return preCollectionUpdateListener; }
		}

		#region Nested type: AbstractListener

		public abstract class AbstractListener : IListener
		{
			private readonly CollectionListeners listeners;

			protected AbstractListener(CollectionListeners listeners)
			{
				this.listeners = listeners;
			}

			#region IListener Members

			public void AddEvent(AbstractCollectionEvent @event, IListener listener)
			{
				listeners.AddEvent(@event, listener);
			}

			#endregion
		}

		#endregion

		#region Nested type: IListener

		public interface IListener
		{
			void AddEvent(AbstractCollectionEvent @event, IListener listener);
		}

		#endregion

		#region Nested type: InitializeCollectionListener

		public class InitializeCollectionListener : DefaultInitializeCollectionEventListener, IListener
		{
			private readonly CollectionListeners listeners;

			public InitializeCollectionListener(CollectionListeners listeners)
			{
				this.listeners = listeners;
			}

			#region IListener Members

			public void AddEvent(AbstractCollectionEvent @event, IListener listener)
			{
				listeners.AddEvent(@event, listener);
			}

			#endregion

			public override async Task OnInitializeCollection(InitializeCollectionEvent @event)
			{
				await base.OnInitializeCollection(@event).ConfigureAwait(false);
				AddEvent(@event, this);
			}
		}

		#endregion

		#region Nested type: PostCollectionRecreateListener

		public class PostCollectionRecreateListener : AbstractListener, IPostCollectionRecreateEventListener
		{
			public PostCollectionRecreateListener(CollectionListeners listeners) : base(listeners) {}

			#region IPostCollectionRecreateEventListener Members

			public Task OnPostRecreateCollection(PostCollectionRecreateEvent @event)
			{
				AddEvent(@event, this);
				return TaskHelper.CompletedTask;
			}

			#endregion
		}

		#endregion

		#region Nested type: PostCollectionRemoveListener

		public class PostCollectionRemoveListener : AbstractListener, IPostCollectionRemoveEventListener
		{
			public PostCollectionRemoveListener(CollectionListeners listeners) : base(listeners) {}

			#region IPostCollectionRemoveEventListener Members

			public Task OnPostRemoveCollection(PostCollectionRemoveEvent @event)
			{
				AddEvent(@event, this);
				return TaskHelper.CompletedTask;
			}

			#endregion
		}

		#endregion

		#region Nested type: PostCollectionUpdateListener

		public class PostCollectionUpdateListener : AbstractListener, IPostCollectionUpdateEventListener
		{
			public PostCollectionUpdateListener(CollectionListeners listeners) : base(listeners) {}

			#region IPostCollectionUpdateEventListener Members

			public Task OnPostUpdateCollection(PostCollectionUpdateEvent @event)
			{
				AddEvent(@event, this);
				return TaskHelper.CompletedTask;
			}

			#endregion
		}

		#endregion

		#region Nested type: PreCollectionRecreateListener

		public class PreCollectionRecreateListener : AbstractListener, IPreCollectionRecreateEventListener
		{
			public PreCollectionRecreateListener(CollectionListeners listeners) : base(listeners) {}

			#region IPreCollectionRecreateEventListener Members

			public Task OnPreRecreateCollection(PreCollectionRecreateEvent @event)
			{
				AddEvent(@event, this);
				return TaskHelper.CompletedTask;
			}

			#endregion
		}

		#endregion

		#region Nested type: PreCollectionRemoveListener

		public class PreCollectionRemoveListener : AbstractListener, IPreCollectionRemoveEventListener
		{
			public PreCollectionRemoveListener(CollectionListeners listeners) : base(listeners) {}

			#region IPreCollectionRemoveEventListener Members

			public Task OnPreRemoveCollection(PreCollectionRemoveEvent @event)
			{
				AddEvent(@event, this);
				return TaskHelper.CompletedTask;
			}

			#endregion
		}

		#endregion

		#region Nested type: PreCollectionUpdateListener

		public class PreCollectionUpdateListener : AbstractListener, IPreCollectionUpdateEventListener
		{
			public PreCollectionUpdateListener(CollectionListeners listeners) : base(listeners) {}

			#region IPreCollectionUpdateEventListener Members

			public Task OnPreUpdateCollection(PreCollectionUpdateEvent @event)
			{
				AddEvent(@event, this);
				return TaskHelper.CompletedTask;
			}

			#endregion
		}

		#endregion
	}
}