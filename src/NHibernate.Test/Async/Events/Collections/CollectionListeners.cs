#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Impl;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Events.Collections
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionListeners
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InitializeCollectionListener : DefaultInitializeCollectionEventListener, IListener
		{
			public override async Task OnInitializeCollectionAsync(InitializeCollectionEvent @event)
			{
				await (base.OnInitializeCollectionAsync(@event));
				AddEvent(@event, this);
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PostCollectionRecreateListener : AbstractListener, IPostCollectionRecreateEventListener
		{
			public Task OnPostRecreateCollectionAsync(PostCollectionRecreateEvent @event)
			{
				try
				{
					OnPostRecreateCollection(@event);
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PostCollectionRemoveListener : AbstractListener, IPostCollectionRemoveEventListener
		{
			public Task OnPostRemoveCollectionAsync(PostCollectionRemoveEvent @event)
			{
				try
				{
					OnPostRemoveCollection(@event);
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PostCollectionUpdateListener : AbstractListener, IPostCollectionUpdateEventListener
		{
			public Task OnPostUpdateCollectionAsync(PostCollectionUpdateEvent @event)
			{
				try
				{
					OnPostUpdateCollection(@event);
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PreCollectionRecreateListener : AbstractListener, IPreCollectionRecreateEventListener
		{
			public Task OnPreRecreateCollectionAsync(PreCollectionRecreateEvent @event)
			{
				try
				{
					OnPreRecreateCollection(@event);
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PreCollectionRemoveListener : AbstractListener, IPreCollectionRemoveEventListener
		{
			public Task OnPreRemoveCollectionAsync(PreCollectionRemoveEvent @event)
			{
				try
				{
					OnPreRemoveCollection(@event);
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class PreCollectionUpdateListener : AbstractListener, IPreCollectionUpdateEventListener
		{
			public Task OnPreUpdateCollectionAsync(PreCollectionUpdateEvent @event)
			{
				try
				{
					OnPreUpdateCollection(@event);
					return TaskHelper.CompletedTask;
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}
	}
}
#endif
