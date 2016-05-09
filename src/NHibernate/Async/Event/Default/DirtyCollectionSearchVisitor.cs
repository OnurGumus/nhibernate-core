using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary>
	/// A Visitor that determines if a dirty collection was found.
	/// </summary>
	/// <remarks>
	/// <list type = "number">
	///		<listheader>
	///			<description>Reason for dirty collection</description>
	///		</listheader>
	///		<item>
	///			<description>
	///			If it is a new application-instantiated collection, return true (does not occur anymore!)
	///			</description>
	///		</item>
	///		<item>
	///			<description>
	///			If it is a component, recurse.
	///			</description>
	///		</item>
	///		<item>
	///			<description>
	///			If it is a wrapped collection, ask the collection entry.
	///			</description>
	///		</item>
	/// </list>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DirtyCollectionSearchVisitor : AbstractVisitor
	{
		internal override async Task<object> ProcessCollectionAsync(object collection, CollectionType type)
		{
			if (collection != null)
			{
				ISessionImplementor session = Session;
				IPersistentCollection persistentCollection;
				if (type.IsArrayType)
				{
					persistentCollection = session.PersistenceContext.GetCollectionHolder(collection);
				// if no array holder we found an unwrappered array (this can't occur,
				// because we now always call wrap() before getting to here)
				// return (ah==null) ? true : searchForDirtyCollections(ah, type);
				}
				else
				{
					// if not wrappered yet, its dirty (this can't occur, because
					// we now always call wrap() before getting to here)
					// return ( ! (obj instanceof PersistentCollection) ) ?
					//true : searchForDirtyCollections( (PersistentCollection) obj, type );
					persistentCollection = (IPersistentCollection)collection;
				}

				if (persistentCollection.IsDirty)
				{
					//we need to check even if it was not initialized, because of delayed adds!
					dirty = true;
					return null; //NOTE: EARLY EXIT!
				}
			}

			return null;
		}
	}
}