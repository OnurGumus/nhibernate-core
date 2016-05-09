using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Abstract superclass of algorithms that walk a tree of property values of an entity, and
	/// perform specific functionality for collections, components and associated entities. 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractVisitor
	{
		internal virtual async Task<object> ProcessComponentAsync(object component, IAbstractComponentType componentType)
		{
			if (component != null)
			{
				await (ProcessValuesAsync(await (componentType.GetPropertyValuesAsync(component, session)), componentType.Subtypes));
			}

			return null;
		}

		internal async Task<object> ProcessValueAsync(object value, IType type)
		{
			if (type.IsCollectionType)
			{
				//even process null collections
				return await (ProcessCollectionAsync(value, (CollectionType)type));
			}
			else if (type.IsEntityType)
			{
				return ProcessEntity(value, (EntityType)type);
			}
			else if (type.IsComponentType)
			{
				return await (ProcessComponentAsync(value, (IAbstractComponentType)type));
			}
			else
			{
				return null;
			}
		}

		internal async Task ProcessValuesAsync(object[] values, IType[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (IncludeProperty(values, i))
					await (ProcessValueAsync(i, values, types));
			}
		}

		public async Task ProcessEntityPropertyValuesAsync(object[] values, IType[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (IncludeEntityProperty(values, i))
				{
					await (ProcessValueAsync(i, values, types));
				}
			}
		}

		internal virtual async Task ProcessAsync(object obj, IEntityPersister persister)
		{
			await (ProcessEntityPropertyValuesAsync(persister.GetPropertyValues(obj, Session.EntityMode), persister.PropertyTypes));
		}

		internal virtual async Task ProcessValueAsync(int i, object[] values, IType[] types)
		{
			await (ProcessValueAsync(values[i], types[i]));
		}
	}
}