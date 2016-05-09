using NHibernate.Id;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Engine
{
	/// <summary> Algorithms related to foreign key constraint transparency </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class ForeignKeys
	{
		public static async Task<bool> IsTransientAsync(string entityName, object entity, bool ? assumed, ISessionImplementor session)
		{
			if (Equals(Intercept.LazyPropertyInitializer.UnfetchedProperty, entity))
			{
				// an unfetched association can only point to
				// an entity that already exists in the db
				return false;
			}

			// let the interceptor inspect the instance to decide
			bool ? isUnsaved = session.Interceptor.IsTransient(entity);
			if (isUnsaved.HasValue)
				return isUnsaved.Value;
			// let the persister inspect the instance to decide
			IEntityPersister persister = session.GetEntityPersister(entityName, entity);
			isUnsaved = await (persister.IsTransientAsync(entity, session));
			if (isUnsaved.HasValue)
				return isUnsaved.Value;
			// we use the assumed value, if there is one, to avoid hitting
			// the database
			if (assumed.HasValue)
				return assumed.Value;
			if (persister.IdentifierGenerator is Assigned)
			{
				// When using assigned identifiers we cannot tell if an entity
				// is transient or detached without querying the database.
				// This could potentially cause Select N+1 in cascaded saves, so warn the user.
				log.Warn("Unable to determine if " + entity.ToString() + " with assigned identifier " + await (persister.GetIdentifierAsync(entity, session.EntityMode)) + " is transient or detached; querying the database." + " Use explicit Save() or Update() in session to prevent this.");
			}

			// hit the database, after checking the session cache for a snapshot
			System.Object[] snapshot = await (session.PersistenceContext.GetDatabaseSnapshotAsync(await (persister.GetIdentifierAsync(entity, session.EntityMode)), persister));
			return snapshot == null;
		}

		public static async Task<bool> IsNotTransientAsync(string entityName, System.Object entity, bool ? assumed, ISessionImplementor session)
		{
			if (entity.IsProxy())
				return true;
			if (session.PersistenceContext.IsEntryFor(entity))
				return true;
			return !await (IsTransientAsync(entityName, entity, assumed, session));
		}

		public static async Task<object> GetEntityIdentifierIfNotUnsavedAsync(string entityName, object entity, ISessionImplementor session)
		{
			if (entity == null)
			{
				return null;
			}
			else
			{
				object id = session.GetContextEntityIdentifier(entity);
				if (id == null)
				{
					// context-entity-identifier returns null explicitly if the entity
					// is not associated with the persistence context; so make some deeper checks...
					/***********************************************/
					// NH-479 (very dirty patch)
					if (entity.GetType().IsPrimitive)
						return entity;
					/**********************************************/
					if (await (IsTransientAsync(entityName, entity, false, session)))
					{
						/***********************************************/
						// TODO NH verify the behavior of NH607 test
						// these lines are only to pass test NH607 during PersistenceContext porting
						// i'm not secure that NH607 is a test for a right behavior
						EntityEntry entry = session.PersistenceContext.GetEntry(entity);
						if (entry != null)
							return entry.Id;
						// the check was put here to have les possible impact
						/**********************************************/
						entityName = entityName ?? session.GuessEntityName(entity);
						string entityString = entity.ToString();
						throw new TransientObjectException(string.Format("object references an unsaved transient instance - save the transient instance before flushing or set cascade action for the property to something that would make it autosave. Type: {0}, Entity: {1}", entityName, entityString));
					}

					id = await (session.GetEntityPersister(entityName, entity).GetIdentifierAsync(entity, session.EntityMode));
				}

				return id;
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class Nullifier
		{
			private async Task<bool> IsNullifiableAsync(string entityName, object obj)
			{
				//if (obj == org.hibernate.intercept.LazyPropertyInitializer_Fields.UNFETCHED_PROPERTY)
				//  return false; //this is kinda the best we can do...
				if (obj.IsProxy())
				{
					INHibernateProxy proxy = obj as INHibernateProxy;
					// if its an uninitialized proxy it can't be transient
					ILazyInitializer li = proxy.HibernateLazyInitializer;
					if (await (li.GetImplementationAsync(session)) == null)
					{
						return false;
					// ie. we never have to null out a reference to
					// an uninitialized proxy
					}
					else
					{
						//unwrap it
						obj = await (li.GetImplementationAsync());
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
					return await (IsTransientAsync(entityName, obj, null, session));
				}
				else
				{
					return entityEntry.IsNullifiable(isEarlyInsert, session);
				}
			}

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
						return await (IsNullifiableAsync(entityName, value)) ? null : value;
					}
				}
				else if (type.IsAnyType)
				{
					return await (IsNullifiableAsync(null, value)) ? null : value;
				}
				else if (type.IsComponentType)
				{
					IAbstractComponentType actype = (IAbstractComponentType)type;
					object[] subvalues = await (actype.GetPropertyValuesAsync(value, session));
					IType[] subtypes = actype.Subtypes;
					bool substitute = false;
					for (int i = 0; i < subvalues.Length; i++)
					{
						object replacement = await (NullifyTransientReferencesAsync(subvalues[i], subtypes[i]));
						if (replacement != subvalues[i])
						{
							substitute = true;
							subvalues[i] = replacement;
						}
					}

					if (substitute)
						actype.SetPropertyValues(value, subvalues, session.EntityMode);
					return value;
				}
				else
				{
					return value;
				}
			}

			public async Task NullifyTransientReferencesAsync(object[] values, IType[] types)
			{
				for (int i = 0; i < types.Length; i++)
				{
					values[i] = await (NullifyTransientReferencesAsync(values[i], types[i]));
				}
			}
		}
	}
}