#if NET_4_5
using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// When an entity is passed to replicate(), and there is an existing row, we must
	/// inspect all its collections and
	/// 1. associate any uninitialized PersistentCollections with this session
	/// 2. associate any initialized PersistentCollections with this session, using the existing snapshot
	/// 3. execute a collection removal (SQL DELETE) for each null collection property or "new" collection 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OnReplicateVisitor : ReattachVisitor
	{
		internal override async Task<object> ProcessCollectionAsync(object collection, CollectionType type)
		{
			if (collection == CollectionType.UnfetchedCollection)
			{
				return null;
			}

			IEventSource session = Session;
			ICollectionPersister persister = session.Factory.GetCollectionPersister(type.Role);
			if (isUpdate)
			{
				await (RemoveCollectionAsync(persister, ExtractCollectionKeyFromOwner(persister), session));
			}

			IPersistentCollection wrapper = collection as IPersistentCollection;
			if (wrapper != null)
			{
				await (wrapper.SetCurrentSessionAsync(session));
				if (wrapper.WasInitialized)
				{
					session.PersistenceContext.AddNewCollection(persister, wrapper);
				}
				else
				{
					ReattachCollection(wrapper, type);
				}
			}
			else
			{
			// otherwise a null or brand new collection
			// this will also (inefficiently) handle arrays, which
			// have no snapshot, so we can't do any better
			//processArrayOrNewCollection(collection, type);
			}

			return null;
		}
	}
}
#endif
