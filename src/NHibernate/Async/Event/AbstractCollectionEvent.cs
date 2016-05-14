#if NET_4_5
using System;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractCollectionEvent : AbstractEvent
	{
		protected static Task<object> GetLoadedOwnerIdOrNullAsync(IPersistentCollection collection, IEventSource source)
		{
			return source.PersistenceContext.GetLoadedCollectionOwnerIdOrNullAsync(collection);
		}
	}
}
#endif
