#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using System.Text;
using System.Xml;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class EntityType : AbstractType, IAssociationType
	{
		public override async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			IEntityPersister persister = factory.GetEntityPersister(associatedEntityName);
			if (!persister.CanExtractIdOutOfEntity)
			{
				return await (base.IsEqualAsync(x, y, entityMode));
			}

			object xid;
			if (x.IsProxy())
			{
				INHibernateProxy proxy = x as INHibernateProxy;
				xid = proxy.HibernateLazyInitializer.Identifier;
			}
			else
			{
				xid = await (persister.GetIdentifierAsync(x, entityMode));
			}

			object yid;
			if (y.IsProxy())
			{
				INHibernateProxy proxy = y as INHibernateProxy;
				yid = proxy.HibernateLazyInitializer.Identifier;
			}
			else
			{
				yid = await (persister.GetIdentifierAsync(y, entityMode));
			}

			return await (persister.IdentifierType.IsEqualAsync(xid, yid, entityMode, factory));
		}

		/// <summary> Two entities are considered the same when their instances are the same. </summary>
		/// <param name = "x">One entity instance </param>
		/// <param name = "y">Another entity instance </param>
		/// <param name = "entityMode">The entity mode. </param>
		/// <returns> True if x == y; false otherwise. </returns>
		public override Task<bool> IsSameAsync(object x, object y, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<bool>(IsSame(x, y, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public override Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
		{
			return NullSafeGetAsync(rs, new string[]{name}, session, owner);
		}

		/// <summary> 
		/// Get the identifier value of an instance or proxy.
		/// <p/>
		/// Intended only for loggin purposes!!!
		/// </summary>
		/// <param name = "obj">The object from which to extract the identifier.</param>
		/// <param name = "persister">The entity persister </param>
		/// <param name = "entityMode">The entity mode </param>
		/// <returns> The extracted identifier. </returns>
		private static async Task<object> GetIdentifierAsync(object obj, IEntityPersister persister, EntityMode entityMode)
		{
			if (obj.IsProxy())
			{
				INHibernateProxy proxy = obj as INHibernateProxy;
				ILazyInitializer li = proxy.HibernateLazyInitializer;
				return li.Identifier;
			}
			else
			{
				return await (persister.GetIdentifierAsync(obj, entityMode));
			}
		}

		protected internal async Task<object> GetIdentifierAsync(object value, ISessionImplementor session)
		{
			if (IsNotEmbedded(session))
			{
				return value;
			}

			return await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(GetAssociatedEntityName(), value, session)); //tolerates nulls
		}

		protected internal async Task<object> GetReferenceValueAsync(object value, ISessionImplementor session)
		{
			if (IsNotEmbedded(session))
			{
				return value;
			}

			if (value == null)
			{
				return null;
			}
			else if (IsReferenceToPrimaryKey)
			{
				return await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(GetAssociatedEntityName(), value, session)); //tolerates nulls
			}
			else
			{
				IEntityPersister entityPersister = session.Factory.GetEntityPersister(GetAssociatedEntityName());
				object propertyValue = entityPersister.GetPropertyValue(value, uniqueKeyPropertyName, session.EntityMode);
				// We now have the value of the property-ref we reference.  However,
				// we need to dig a little deeper, as that property might also be
				// an entity type, in which case we need to resolve its identitifier
				IType type = entityPersister.GetPropertyType(uniqueKeyPropertyName);
				if (type.IsEntityType)
				{
					propertyValue = await (((EntityType)type).GetReferenceValueAsync(propertyValue, session));
				}

				return propertyValue;
			}
		}

		public override async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			if (value == null)
			{
				return "null";
			}

			IEntityPersister persister = factory.GetEntityPersister(associatedEntityName);
			StringBuilder result = new StringBuilder().Append(associatedEntityName);
			if (persister.HasIdentifierProperty)
			{
				EntityMode? entityMode = persister.GuessEntityMode(value);
				object id;
				if (!entityMode.HasValue)
				{
					if (isEmbeddedInXML)
						throw new InvalidCastException(value.GetType().FullName);
					id = value;
				}
				else
				{
					id = await (GetIdentifierAsync(value, persister, entityMode.Value));
				}

				result.Append('#').Append(await (persister.IdentifierType.ToLoggableStringAsync(id, factory)));
			}

			return result.ToString();
		}

		public override Task<object> DeepCopyAsync(object value, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			try
			{
				return Task.FromResult<object>(DeepCopy(value, entityMode, factory));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copyCache)
		{
			if (original == null)
			{
				return null;
			}

			object cached = copyCache[original];
			if (cached != null)
			{
				return cached;
			}
			else
			{
				if (original == target)
				{
					return target;
				}

				if (session.GetContextEntityIdentifier(original) == null && await (ForeignKeys.IsTransientAsync(associatedEntityName, original, false, session)))
				{
					object copy = await (session.Factory.GetEntityPersister(associatedEntityName).InstantiateAsync(null, session.EntityMode));
					//TODO: should this be Session.instantiate(Persister, ...)?
					copyCache.Add(original, copy);
					return copy;
				}
				else
				{
					object id = await (GetReferenceValueAsync(original, session));
					if (id == null)
					{
						throw new AssertionFailure("non-transient entity has a null id");
					}

					id = await (GetIdentifierOrUniqueKeyType(session.Factory).ReplaceAsync(id, null, session, owner, copyCache));
					return await (ResolveIdentifierAsync(id, session, owner));
				}
			}
		}

		/// <summary>
		/// Converts the id contained in the <see cref = "DbDataReader"/> to an object.
		/// </summary>
		/// <param name = "rs">The <see cref = "DbDataReader"/> that contains the query results.</param>
		/// <param name = "names">A string array of column names that contain the id.</param>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this is occurring in.</param>
		/// <param name = "owner">The object that this Entity will be a part of.</param>
		/// <returns>
		/// An instance of the object or <see langword = "null"/> if the identifer was null.
		/// </returns>
		public override sealed async Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return await (ResolveIdentifierAsync(await (HydrateAsync(rs, names, session, owner)), session, owner));
		}

		public abstract override Task<object> HydrateAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner);
		/// <summary>
		/// Resolves the identifier to the actual object.
		/// </summary>
		protected async Task<object> ResolveIdentifierAsync(object id, ISessionImplementor session)
		{
			string entityName = GetAssociatedEntityName();
			bool isProxyUnwrapEnabled = unwrapProxy && session.Factory.GetEntityPersister(entityName).IsInstrumented(session.EntityMode);
			object proxyOrEntity = await (session.InternalLoadAsync(entityName, id, eager, IsNullable && !isProxyUnwrapEnabled));
			if (proxyOrEntity.IsProxy())
			{
				INHibernateProxy proxy = (INHibernateProxy)proxyOrEntity;
				proxy.HibernateLazyInitializer.Unwrap = isProxyUnwrapEnabled;
			}

			return proxyOrEntity;
		}

		/// <summary>
		/// Resolve an identifier or unique key value
		/// </summary>
		/// <param name = "value"></param>
		/// <param name = "session"></param>
		/// <param name = "owner"></param>
		/// <returns></returns>
		public override Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(ResolveIdentifier(value, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			IEntityPersister persister = factory.GetEntityPersister(associatedEntityName);
			if (!persister.CanExtractIdOutOfEntity)
			{
				return await (base.GetHashCodeAsync(x, entityMode));
			}

			object id;
			if (x.IsProxy())
			{
				INHibernateProxy proxy = x as INHibernateProxy;
				id = proxy.HibernateLazyInitializer.Identifier;
			}
			else
			{
				id = await (persister.GetIdentifierAsync(x, entityMode));
			}

			return await (persister.IdentifierType.GetHashCodeAsync(id, entityMode, factory));
		}

		public override Task<int> CompareAsync(object x, object y, EntityMode? entityMode)
		{
			try
			{
				return Task.FromResult<int>(Compare(x, y, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<int>(ex);
			}
		}
	}
}
#endif
