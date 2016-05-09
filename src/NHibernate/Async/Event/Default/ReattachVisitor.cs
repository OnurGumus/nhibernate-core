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