using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Intercept;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Tuple.Entity
{
	/// <summary> Support for tuplizers relating to entities. </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEntityTuplizer : IEntityTuplizer
	{
		public async Task<object> GetIdentifierAsync(object entity)
		{
			object id;
			if (entityMetamodel.IdentifierProperty.IsEmbedded)
			{
				id = entity;
			}
			else
			{
				if (idGetter == null)
				{
					if (identifierMapperType == null)
					{
						throw new HibernateException("The class has no identifier property: " + EntityName);
					}
					else
					{
						ComponentType copier = (ComponentType)entityMetamodel.IdentifierProperty.Type;
						id = await (copier.InstantiateAsync(EntityMode));
						copier.SetPropertyValues(id, await (identifierMapperType.GetPropertyValuesAsync(entity, EntityMode)), EntityMode);
					}
				}
				else
				{
					id = idGetter.Get(entity);
				}
			}

			return id;
		}

		public async Task SetIdentifierAsync(object entity, object id)
		{
			if (entityMetamodel.IdentifierProperty.IsEmbedded)
			{
				if (entity != id)
				{
					IAbstractComponentType copier = (IAbstractComponentType)entityMetamodel.IdentifierProperty.Type;
					copier.SetPropertyValues(entity, await (copier.GetPropertyValuesAsync(id, EntityMode)), EntityMode);
				}
			}
			else if (idSetter != null)
			{
				idSetter.Set(entity, id);
			}
		}

		public async Task ResetIdentifierAsync(object entity, object currentId, object currentVersion)
		{
			if (!(entityMetamodel.IdentifierProperty.IdentifierGenerator is Assigned))
			{
				//reset the id
				object result = entityMetamodel.IdentifierProperty.UnsavedValue.GetDefaultValue(currentId);
				await (SetIdentifierAsync(entity, result));
				//reset the version
				VersionProperty versionProperty = entityMetamodel.VersionProperty;
				if (entityMetamodel.IsVersioned)
				{
					SetPropertyValue(entity, entityMetamodel.VersionPropertyIndex, versionProperty.UnsavedValue.GetDefaultValue(currentVersion));
				}
			}
		}

		public async Task<object> InstantiateAsync(object id)
		{
			object result = Instantiator.Instantiate(id);
			if (id != null)
			{
				await (SetIdentifierAsync(result, id));
			}

			return result;
		}

		public async Task<object> InstantiateAsync()
		{
			return await (InstantiateAsync(null));
		}

		public virtual async Task<object[]> GetPropertyValuesToInsertAsync(object entity, IDictionary mergeMap, ISessionImplementor session)
		{
			int span = entityMetamodel.PropertySpan;
			object[] result = new object[span];
			for (int j = 0; j < span; j++)
			{
				result[j] = await (getters[j].GetForInsertAsync(entity, mergeMap, session));
			}

			return result;
		}
	}
}