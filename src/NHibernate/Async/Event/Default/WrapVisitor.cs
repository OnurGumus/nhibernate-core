#if NET_4_5
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event.Default
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class WrapVisitor : ProxyVisitor
	{
		internal override async Task ProcessAsync(object obj, IEntityPersister persister)
		{
			EntityMode entityMode = Session.EntityMode;
			object[] values = persister.GetPropertyValues(obj, entityMode);
			IType[] types = persister.PropertyTypes;
			await (ProcessEntityPropertyValuesAsync(values, types));
			if (SubstitutionRequired)
			{
				persister.SetPropertyValues(obj, values, entityMode);
			}
		}

		internal override Task<object> ProcessCollectionAsync(object collection, CollectionType collectionType)
		{
			try
			{
				return Task.FromResult<object>(ProcessCollection(collection, collectionType));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		internal override async Task ProcessValueAsync(int i, object[] values, IType[] types)
		{
			object result = await (ProcessValueAsync(values[i], types[i]));
			if (result != null)
			{
				substitute = true;
				values[i] = result;
			}
		}

		internal override async Task<object> ProcessComponentAsync(object component, IAbstractComponentType componentType)
		{
			if (component != null)
			{
				object[] values = await (componentType.GetPropertyValuesAsync(component, Session));
				IType[] types = componentType.Subtypes;
				bool substituteComponent = false;
				for (int i = 0; i < types.Length; i++)
				{
					object result = await (ProcessValueAsync(values[i], types[i]));
					if (result != null)
					{
						values[i] = result;
						substituteComponent = true;
					}
				}

				if (substituteComponent)
				{
					componentType.SetPropertyValues(component, values, Session.EntityMode);
				}
			}

			return null;
		}
	}
}
#endif
