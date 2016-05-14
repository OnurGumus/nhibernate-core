#if NET_4_5
using NHibernate.Collection;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// When an entity is passed to update(), we must inspect all its collections and
	/// 1. associate any uninitialized PersistentCollections with this session
	/// 2. associate any initialized PersistentCollections with this session, using the existing snapshot
	/// 3. execute a collection removal (SQL DELETE) for each null collection property or "new" collection 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OnUpdateVisitor : ReattachVisitor
	{
		internal override async Task<object> ProcessCollectionAsync(object collection, CollectionType type)
		{
			if (collection == CollectionType.UnfetchedCollection)
			{
				return null;
			}

			IEventSource session = Session;
			ICollectionPersister persister = session.Factory.GetCollectionPersister(type.Role);
			object collectionKey = ExtractCollectionKeyFromOwner(persister);
			IPersistentCollection wrapper = collection as IPersistentCollection;
			if (wrapper != null)
			{
				if (await (wrapper.SetCurrentSessionAsync(session)))
				{
					//a "detached" collection!
					if (!IsOwnerUnchanged(wrapper, persister, collectionKey))
					{
						await (RemoveCollectionAsync(persister, collectionKey, session));
					}

					ReattachCollection(wrapper, type);
				}
				else
				{
					await (RemoveCollectionAsync(persister, collectionKey, session));
				}
			}
			else
			{
				await (RemoveCollectionAsync(persister, collectionKey, session));
			}

			return null;
		}
	}
}
#endif
