#if NET_4_5
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Abstract superclass of algorithms that walk a tree of property values of an entity, and
	/// perform specific functionality for collections, components and associated entities. 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractVisitor
	{
		/// <summary> Dispatch each property value to ProcessValue(). </summary>
		/// <param name = "values"> </param>
		/// <param name = "types"> </param>
		internal async Task ProcessValuesAsync(object[] values, IType[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (IncludeProperty(values, i))
					await (ProcessValueAsync(i, values, types));
			}
		}

		internal virtual Task ProcessValueAsync(int i, object[] values, IType[] types)
		{
			return ProcessValueAsync(values[i], types[i]);
		}

		/// <summary> 
		/// Visit a property value. Dispatch to the correct handler for the property type.
		/// </summary>
		/// <param name = "value"> </param>
		/// <param name = "type"> </param>
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

		/// <summary>
		/// Visit a component. Dispatch each property to <see cref = "ProcessValues"/>
		/// </summary>
		/// <param name = "component"></param>
		/// <param name = "componentType"></param>
		/// <returns></returns>
		internal virtual async Task<object> ProcessComponentAsync(object component, IAbstractComponentType componentType)
		{
			if (component != null)
			{
				await (ProcessValuesAsync(await (componentType.GetPropertyValuesAsync(component, session)), componentType.Subtypes));
			}

			return null;
		}

		/// <summary>
		/// Visit a collection. Default superclass implementation is a no-op.
		/// </summary>
		/// <param name = "value"></param>
		/// <param name = "collectionType"></param>
		/// <returns></returns>
		internal virtual Task<object> ProcessCollectionAsync(object value, CollectionType collectionType)
		{
			try
			{
				return Task.FromResult<object>(ProcessCollection(value, collectionType));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <summary>
		/// Walk the tree starting from the given entity.
		/// </summary>
		/// <param name = "obj"></param>
		/// <param name = "persister"></param>
		internal virtual async Task ProcessAsync(object obj, IEntityPersister persister)
		{
			await (ProcessEntityPropertyValuesAsync(persister.GetPropertyValues(obj, Session.EntityMode), persister.PropertyTypes));
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
	}
}
#endif
