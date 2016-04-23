using System.Threading.Tasks;
using NHibernate.Persister.Entity;
using NHibernate.Type;

namespace NHibernate.Event.Default
{
	/// <summary> 
	/// Abstract superclass of algorithms that walk a tree of property values of an entity, and
	/// perform specific functionality for collections, components and associated entities. 
	/// </summary>
	public abstract class AbstractVisitor
	{
		private readonly IEventSource session;

		public AbstractVisitor(IEventSource session)
		{
			this.session = session;
		}

		public IEventSource Session
		{
			get { return session; }
		}

		/// <summary> Dispatch each property value to ProcessValue(). </summary>
		/// <param name="values"> </param>
		/// <param name="types"> </param>
		internal async Task ProcessValues(object[] values, IType[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (IncludeProperty(values, i))
					await ProcessValue(i, values, types).ConfigureAwait(false);
			}
		}

		internal virtual Task ProcessValue(int i, object[] values, IType[] types)
		{
			return ProcessValue(values[i], types[i]);
		}

		/// <summary> 
		/// Visit a property value. Dispatch to the correct handler for the property type.
		/// </summary>
		/// <param name="value"> </param>
		/// <param name="type"> </param>
		internal async Task<object> ProcessValue(object value, IType type)
		{
			if (type.IsCollectionType)
			{
				//even process null collections
				return await ProcessCollection(value, (CollectionType)type).ConfigureAwait(false);
			}
			else if (type.IsEntityType)
			{
				return ProcessEntity(value, (EntityType)type);
			}
			else if (type.IsComponentType)
			{
				return await ProcessComponent(value, (IAbstractComponentType)type).ConfigureAwait(false);
			}
			else
			{
				return null;
			} 
		}

		/// <summary>
		/// Visit a component. Dispatch each property to <see cref="ProcessValues"/>
		/// </summary>
		/// <param name="component"></param>
		/// <param name="componentType"></param>
		/// <returns></returns>
		internal virtual async Task<object> ProcessComponent(object component, IAbstractComponentType componentType)
		{
			if (component != null)
			{
				await ProcessValues(await componentType.GetPropertyValues(component, session).ConfigureAwait(false), componentType.Subtypes).ConfigureAwait(false);
			}
			return null;
		}

		/// <summary>
		///  Visit a many-to-one or one-to-one associated entity. Default superclass implementation is a no-op.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="entityType"></param>
		/// <returns></returns>
		internal virtual object ProcessEntity(object value, EntityType entityType)
		{
			return null;
		}

		/// <summary>
		/// Visit a collection. Default superclass implementation is a no-op.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="collectionType"></param>
		/// <returns></returns>
		internal virtual Task<object> ProcessCollection(object value, CollectionType collectionType)
		{
			return Task.FromResult<object>(null);
		}

		/// <summary>
		/// Walk the tree starting from the given entity.
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="persister"></param>
		internal virtual Task Process(object obj, IEntityPersister persister)
		{
			return ProcessEntityPropertyValues(persister.GetPropertyValues(obj, Session.EntityMode), persister.PropertyTypes);
		}

		public async Task ProcessEntityPropertyValues(object[] values, IType[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (IncludeEntityProperty(values, i))
				{
					await ProcessValue(i, values, types).ConfigureAwait(false);
				}
			}
		}

		internal virtual bool IncludeEntityProperty(object[] values, int i)
		{
			return IncludeProperty(values, i);
		}

		internal bool IncludeProperty(object[] values, int i)
		{
			return !Equals(Intercept.LazyPropertyInitializer.UnfetchedProperty, values[i]);
		}
	}
}
