using System.Threading.Tasks;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Process collections reachable from an entity. 
	/// This visitor assumes that wrap was already performed for the entity.
	/// </summary>
	public class FlushVisitor : AbstractVisitor
	{
		private readonly object owner;

		public FlushVisitor(IEventSource session, object owner)
			: base(session)
		{
			this.owner = owner;
		}

		internal override async Task<object> ProcessCollection(object collection, CollectionType type)
		{
			if (collection == CollectionType.UnfetchedCollection)
			{
				return null;
			}

			if (collection != null)
			{
				IPersistentCollection coll;
				if (type.IsArrayType)
				{
					coll = Session.PersistenceContext.GetCollectionHolder(collection);
				}
				else
				{
					coll = (IPersistentCollection)collection;
				}

				await Collections.ProcessReachableCollection(coll, type, owner, Session).ConfigureAwait(false);
			}
			return null;
		}
	}
}
