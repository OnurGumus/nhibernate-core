using System;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using System.Threading.Tasks;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractCollectionEvent : AbstractEvent
	{
		protected static async Task<object> GetLoadedOwnerIdOrNullAsync(IPersistentCollection collection, IEventSource source)
		{
			return await (source.PersistenceContext.GetLoadedCollectionOwnerIdOrNullAsync(collection));
		}
	}
}