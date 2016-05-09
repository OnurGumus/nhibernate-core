using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Evict any collections referenced by the object from the session cache.
	/// This will NOT pick up any collections that were dereferenced, so they
	/// will be deleted (suboptimal but not exactly incorrect). 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EvictVisitor : AbstractVisitor
	{
		internal override async Task<object> ProcessCollectionAsync(object collection, CollectionType type)
		{
			if (collection != null)
				await (EvictCollectionAsync(collection, type));
			return null;
		}

		private async Task EvictCollectionAsync(IPersistentCollection collection)
		{
			CollectionEntry ce = (CollectionEntry)Session.PersistenceContext.CollectionEntries[collection];
			Session.PersistenceContext.CollectionEntries.Remove(collection);
			if (log.IsDebugEnabled)
				log.Debug("evicting collection: " + await (MessageHelper.CollectionInfoStringAsync(ce.LoadedPersister, collection, ce.LoadedKey, Session)));
			if (ce.LoadedPersister != null && ce.LoadedKey != null)
			{
				//TODO: is this 100% correct?
				Session.PersistenceContext.CollectionsByKey.Remove(new CollectionKey(ce.LoadedPersister, ce.LoadedKey, Session.EntityMode));
			}
		}

		public virtual async Task EvictCollectionAsync(object value, CollectionType type)
		{
			IPersistentCollection pc;
			if (type.IsArrayType)
			{
				pc = Session.PersistenceContext.RemoveCollectionHolder(value);
			}
			else if (value is IPersistentCollection)
			{
				pc = (IPersistentCollection)value;
			}
			else
			{
				return; //EARLY EXIT!
			}

			IPersistentCollection collection = pc;
			if (collection.UnsetSession(Session))
				await (EvictCollectionAsync(collection));
		}
	}
}