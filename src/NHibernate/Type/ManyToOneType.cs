using System;
using System.Data;
using System.Threading.Tasks;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlTypes;
using NHibernate.Util;

namespace NHibernate.Type
{
	/// <summary>
	/// A many-to-one association to an entity
	/// </summary>
	[Serializable]
	public class ManyToOneType : EntityType
	{
		private readonly bool ignoreNotFound;
		private readonly bool isLogicalOneToOne;

		public ManyToOneType(string className)
			: this(className, false)
		{
		}

		public ManyToOneType(string className, bool lazy)
			: base(className, null, !lazy, true, false)
		{
			ignoreNotFound = false;
			isLogicalOneToOne = false;
		}

		public ManyToOneType(string entityName, string uniqueKeyPropertyName, bool lazy, bool unwrapProxy, bool isEmbeddedInXML, bool ignoreNotFound, bool isLogicalOneToOne)
			: base(entityName, uniqueKeyPropertyName, !lazy, isEmbeddedInXML, unwrapProxy)
		{
			this.ignoreNotFound = ignoreNotFound;
			this.isLogicalOneToOne = isLogicalOneToOne;
		}

		public override int GetColumnSpan(IMapping mapping)
		{
			// our column span is the number of columns in the PK
			return GetIdentifierOrUniqueKeyType(mapping).GetColumnSpan(mapping);
		}

		public override SqlType[] SqlTypes(IMapping mapping)
		{
			return GetIdentifierOrUniqueKeyType(mapping).SqlTypes(mapping);
		}

		public override async Task NullSafeSet(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			await GetIdentifierOrUniqueKeyType(session.Factory)
				.NullSafeSet(st, await GetReferenceValue(value, session).ConfigureAwait(false), index, settable, session).ConfigureAwait(false);
		}

		public override async Task NullSafeSet(IDbCommand cmd, object value, int index, ISessionImplementor session)
		{
			await GetIdentifierOrUniqueKeyType(session.Factory)
				.NullSafeSet(cmd, await GetReferenceValue(value, session).ConfigureAwait(false), index, session).ConfigureAwait(false);
		}

		public override bool IsOneToOne
		{
			get { return false; }
		}

		public override bool IsLogicalOneToOne()
		{
			return isLogicalOneToOne;
		}

		public override ForeignKeyDirection ForeignKeyDirection
		{
			get { return ForeignKeyDirection.ForeignKeyFromParent; }
		}

		/// <summary>
		/// Hydrates the Identifier from <see cref="IDataReader"/>.
		/// </summary>
		/// <param name="rs">The <see cref="IDataReader"/> that contains the query results.</param>
		/// <param name="names">A string array of column names to read from.</param>
		/// <param name="session">The <see cref="ISessionImplementor"/> this is occurring in.</param>
		/// <param name="owner">The object that this Entity will be a part of.</param>
		/// <returns>
		/// An instantiated object that used as the identifier of the type.
		/// </returns>
		public override async Task<object> Hydrate(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			// return the (fully resolved) identifier value, but do not resolve
			// to the actual referenced entity instance
			// NOTE: the owner of the association is not really the owner of the id!
			object id = await GetIdentifierOrUniqueKeyType(session.Factory)
				.NullSafeGet(rs, names, session, owner).ConfigureAwait(false);
			ScheduleBatchLoadIfNeeded(id, session);
			return id;
		}

		private void ScheduleBatchLoadIfNeeded(object id, ISessionImplementor session)
		{
			//cannot batch fetch by unique key (property-ref associations)
			if (uniqueKeyPropertyName == null && id != null)
			{
				IEntityPersister persister = session.Factory.GetEntityPersister(GetAssociatedEntityName());
				EntityKey entityKey = session.GenerateEntityKey(id, persister);
				if (!session.PersistenceContext.ContainsEntity(entityKey))
				{
					session.PersistenceContext.BatchFetchQueue.AddBatchLoadableEntityKey(entityKey);
				}
			}
		}

		public override bool UseLHSPrimaryKey
		{
			get { return false; }
		}

		public override async Task<bool> IsModified(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			if (current == null)
			{
				return old != null;
			}
			if (old == null)
			{
				return true;
			}
			// the ids are fully resolved, so compare them with isDirty(), not isModified()
			return await GetIdentifierOrUniqueKeyType(session.Factory).IsDirty(old, await GetIdentifier(current, session).ConfigureAwait(false), session).ConfigureAwait(false);
		}

		public override async Task<object> Disassemble(object value, ISessionImplementor session, object owner)
		{
			if (IsNotEmbedded(session))
			{
				return await GetIdentifierType(session).Disassemble(value, session, owner).ConfigureAwait(false);
			}

			if (value == null)
			{
				return null;
			}
			else
			{
				// cache the actual id of the object, not the value of the
				// property-ref, which might not be initialized
				object id = await ForeignKeys.GetEntityIdentifierIfNotUnsaved(GetAssociatedEntityName(), value, session).ConfigureAwait(false);
				if (id == null)
				{
					throw new AssertionFailure("cannot cache a reference to an object with a null id: " + GetAssociatedEntityName());
				}
				return await GetIdentifierType(session).Disassemble(id, session, owner).ConfigureAwait(false);
			}
		}

		public override async Task<object> Assemble(object oid, ISessionImplementor session, object owner)
		{
			//TODO: currently broken for unique-key references (does not detect
			//      change to unique key property of the associated object)

			object id = await AssembleId(oid, session).ConfigureAwait(false);

			if (IsNotEmbedded(session))
			{
				return id;
			}

			if (id == null)
			{
				return null;
			}
			else
			{
				return await ResolveIdentifier(id, session).ConfigureAwait(false);
			}
		}

		public override async Task BeforeAssemble(object oid, ISessionImplementor session)
		{
			ScheduleBatchLoadIfNeeded(await AssembleId(oid, session).ConfigureAwait(false), session);
		}

		private Task<object> AssembleId(object oid, ISessionImplementor session)
		{
			//the owner of the association is not the owner of the id
			return GetIdentifierType(session).Assemble(oid, session, null);
		}

		public override bool IsAlwaysDirtyChecked
		{
			get { return ignoreNotFound; }
		}

		public override async Task<bool> IsDirty(object old, object current, ISessionImplementor session)
		{
			if (IsSame(old, current, session.EntityMode))
			{
				return false;
			}

			object oldid = await GetIdentifier(old, session).ConfigureAwait(false);
			object newid = await GetIdentifier(current, session).ConfigureAwait(false);
			return await GetIdentifierType(session).IsDirty(oldid, newid, session).ConfigureAwait(false);
		}

		public override async Task<bool> IsDirty(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			if (IsAlwaysDirtyChecked)
			{
				return await IsDirty(old, current, session).ConfigureAwait(false);
			}
			else
			{
				if (IsSame(old, current, session.EntityMode))
				{
					return false;
				}

				object oldid = await GetIdentifier(old, session).ConfigureAwait(false);
				object newid = await GetIdentifier(current, session).ConfigureAwait(false);
				return await GetIdentifierType(session).IsDirty(oldid, newid, checkable, session).ConfigureAwait(false);
			}
		}

		public override bool IsNullable
		{
			get { return ignoreNotFound; }
		}

		public override bool[] ToColumnNullness(object value, IMapping mapping)
		{
			bool[] result = new bool[GetColumnSpan(mapping)];
			if (value != null)
			{
				ArrayHelper.Fill(result, true);
			}
			return result;
		}
	}
}
