﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



using NHibernate.Id;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;

namespace NHibernate.Engine
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public static partial class ForeignKeys
	{

		/// <content>
		/// Contains generated async methods
		/// </content>
		public partial class Nullifier
		{

			/// <summary> 
			/// Nullify all references to entities that have not yet 
			/// been inserted in the database, where the foreign key
			/// points toward that entity
			/// </summary>
			public async Task NullifyTransientReferencesAsync(object[] values, IType[] types)
			{
				for (int i = 0; i < types.Length; i++)
				{
					values[i] = await (NullifyTransientReferencesAsync(values[i], types[i])).ConfigureAwait(false);
				}
			}

			/// <summary> 
			/// Return null if the argument is an "unsaved" entity (ie. 
			/// one with no existing database row), or the input argument 
			/// otherwise. This is how Hibernate avoids foreign key constraint
			/// violations.
			/// </summary>
			private async Task<object> NullifyTransientReferencesAsync(object value, IType type)
			{
				if (value == null)
				{
					return null;
				}
				else if (type.IsEntityType)
				{
					EntityType entityType = (EntityType)type;
					if (entityType.IsOneToOne)
					{
						return value;
					}
					else
					{
						string entityName = entityType.GetAssociatedEntityName();
						return await (IsNullifiableAsync(entityName, value)) .ConfigureAwait(false)? null : value;
					}
				}
				else if (type.IsAnyType)
				{
					return await (IsNullifiableAsync(null, value)) .ConfigureAwait(false)? null : value;
				}
				else if (type.IsComponentType)
				{
					IAbstractComponentType actype = (IAbstractComponentType)type;
					object[] subvalues = actype.GetPropertyValues(value, session);
					IType[] subtypes = actype.Subtypes;
					bool substitute = false;
					for (int i = 0; i < subvalues.Length; i++)
					{
						object replacement = await (NullifyTransientReferencesAsync(subvalues[i], subtypes[i])).ConfigureAwait(false);
						if (replacement != subvalues[i])
						{
							substitute = true;
							subvalues[i] = replacement;
						}
					}
					if (substitute)
						actype.SetPropertyValues(value, subvalues);
					return value;
				}
				else
				{
					return value;
				}
			}

			/// <summary> 
			/// Determine if the object already exists in the database, using a "best guess"
			/// </summary>
			private async Task<bool> IsNullifiableAsync(string entityName, object obj)
			{

				//if (obj == org.hibernate.intercept.LazyPropertyInitializer_Fields.UNFETCHED_PROPERTY)
				//  return false; //this is kinda the best we can do...

				if (obj.IsProxy())
				{
                    INHibernateProxy proxy = obj as INHibernateProxy;
                    
                    // if its an uninitialized proxy it can't be transient
					ILazyInitializer li = proxy.HibernateLazyInitializer;
					if (li.GetImplementation(session) == null)
					{
						return false;
						// ie. we never have to null out a reference to
						// an uninitialized proxy
					}
					else
					{
						//unwrap it
						obj = await (li.GetImplementationAsync()).ConfigureAwait(false);
					}
				}

				// if it was a reference to self, don't need to nullify
				// unless we are using native id generation, in which
				// case we definitely need to nullify
				if (obj == self)
				{
					// TODO H3.2: Different behaviour
					//return isEarlyInsert || (isDelete && session.Factory.Dialect.HasSelfReferentialForeignKeyBug);
					return isEarlyInsert || isDelete;
				}

				// See if the entity is already bound to this session, if not look at the
				// entity identifier and assume that the entity is persistent if the
				// id is not "unsaved" (that is, we rely on foreign keys to keep
				// database integrity)
				EntityEntry entityEntry = session.PersistenceContext.GetEntry(obj);
				if (entityEntry == null)
				{
					return await (IsTransientSlowAsync(entityName, obj, session)).ConfigureAwait(false);
				}
				else
				{
					return entityEntry.IsNullifiable(isEarlyInsert, session);
				}
			}
		}

		/// <summary> 
		/// Is this instance persistent or detached?
		/// </summary>
		/// <remarks>
		/// Hit the database to make the determination.
		/// </remarks>
		public static async Task<bool> IsNotTransientSlowAsync(string entityName, object entity, ISessionImplementor session)
		{
			if (entity.IsProxy())
				return true;
			if (session.PersistenceContext.IsEntryFor(entity))
				return true;
			return !await (IsTransientSlowAsync(entityName, entity, session)).ConfigureAwait(false);
		}

		/// <summary> 
		/// Is this instance, which we know is not persistent, actually transient? 
		/// </summary>
		/// <remarks>
		/// Hit the database to make the determination.
		/// </remarks>
		public static async Task<bool> IsTransientSlowAsync(string entityName, object entity, ISessionImplementor session)
		{
			return IsTransientFast(entityName, entity, session) ??
			       await (HasDbSnapshotAsync(entityName, entity, session)).ConfigureAwait(false);
		}

		static async Task<bool> HasDbSnapshotAsync(string entityName, object entity, ISessionImplementor session)
		{
			IEntityPersister persister = session.GetEntityPersister(entityName, entity);
			if (persister.IdentifierGenerator is Assigned)
			{
				// When using assigned identifiers we cannot tell if an entity
				// is transient or detached without querying the database.
				// This could potentially cause Select N+1 in cascaded saves, so warn the user.
				log.Warn(
					"Unable to determine if " + entity.ToString()
					+ " with assigned identifier " + persister.GetIdentifier(entity)
					+ " is transient or detached; querying the database."
					+ " Use explicit Save() or Update() in session to prevent this.");
			}

			// hit the database, after checking the session cache for a snapshot
			System.Object[] snapshot =
				await (session.PersistenceContext.GetDatabaseSnapshotAsync(persister.GetIdentifier(entity), persister)).ConfigureAwait(false);
			return snapshot == null;
		}
	}
}
