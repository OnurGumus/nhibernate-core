#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Event;
using NHibernate.Event.Default;
using NHibernate.Impl;
using System.Threading.Tasks;

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
	}
}
#endif
