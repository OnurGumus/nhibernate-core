#if NET_4_5
using NHibernate.Action;
using NHibernate.Engine;
using NHibernate.Impl;
using NHibernate.Persister.Collection;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary>
	/// Abstract superclass of visitors that reattach collections
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class ReattachVisitor : ProxyVisitor
	{
		internal override async Task<object> ProcessComponentAsync(object component, IAbstractComponentType componentType)
		{
			IType[] types = componentType.Subtypes;
			if (component == null)
			{
				await (ProcessValuesAsync(new object[types.Length], types));
			}
			else
			{
				await (base.ProcessComponentAsync(component, componentType));
			}

			return null;
		}

		/// <summary> 
		/// Schedules a collection for deletion. 
		/// </summary>
		/// <param name = "role">The persister representing the collection to be removed. </param>
		/// <param name = "collectionKey">The collection key (differs from owner-id in the case of property-refs). </param>
		/// <param name = "source">The session from which the request originated. </param>
		internal async Task RemoveCollectionAsync(ICollectionPersister role, object collectionKey, IEventSource source)
		{
			if (log.IsDebugEnabled)
			{
				log.Debug("collection dereferenced while transient " + await (MessageHelper.CollectionInfoStringAsync(role, ownerIdentifier, source.Factory)));
			}

			source.ActionQueue.AddAction(new CollectionRemoveAction(owner, role, collectionKey, false, source));
		}
	}
}
#endif
