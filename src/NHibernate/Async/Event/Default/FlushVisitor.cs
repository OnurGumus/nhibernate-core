#if NET_4_5
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FlushVisitor : AbstractVisitor
	{
		internal override async Task<object> ProcessCollectionAsync(object collection, CollectionType type)
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

				await (Collections.ProcessReachableCollectionAsync(coll, type, owner, Session));
			}

			return null;
		}
	}
}
#endif
