#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ManyToOneType : EntityType
	{
		public override async Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			await (GetIdentifierOrUniqueKeyType(session.Factory).NullSafeSetAsync(st, await (GetReferenceValueAsync(value, session)), index, settable, session));
		}

		public override async Task NullSafeSetAsync(DbCommand cmd, object value, int index, ISessionImplementor session)
		{
			await (GetIdentifierOrUniqueKeyType(session.Factory).NullSafeSetAsync(cmd, await (GetReferenceValueAsync(value, session)), index, session));
		}

		/// <summary>
		/// Hydrates the Identifier from <see cref = "DbDataReader"/>.
		/// </summary>
		/// <param name = "rs">The <see cref = "DbDataReader"/> that contains the query results.</param>
		/// <param name = "names">A string array of column names to read from.</param>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this is occurring in.</param>
		/// <param name = "owner">The object that this Entity will be a part of.</param>
		/// <returns>
		/// An instantiated object that used as the identifier of the type.
		/// </returns>
		public override async Task<object> HydrateAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			// return the (fully resolved) identifier value, but do not resolve
			// to the actual referenced entity instance
			// NOTE: the owner of the association is not really the owner of the id!
			object id = await (GetIdentifierOrUniqueKeyType(session.Factory).NullSafeGetAsync(rs, names, session, owner));
			ScheduleBatchLoadIfNeeded(id, session);
			return id;
		}

		public override async Task<bool> IsModifiedAsync(object old, object current, bool[] checkable, ISessionImplementor session)
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
			return await (GetIdentifierOrUniqueKeyType(session.Factory).IsDirtyAsync(old, await (GetIdentifierAsync(current, session)), session));
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			if (IsNotEmbedded(session))
			{
				return await (GetIdentifierType(session).DisassembleAsync(value, session, owner));
			}

			if (value == null)
			{
				return null;
			}
			else
			{
				// cache the actual id of the object, not the value of the
				// property-ref, which might not be initialized
				object id = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(GetAssociatedEntityName(), value, session));
				if (id == null)
				{
					throw new AssertionFailure("cannot cache a reference to an object with a null id: " + GetAssociatedEntityName());
				}

				return await (GetIdentifierType(session).DisassembleAsync(id, session, owner));
			}
		}

		public override async Task<object> AssembleAsync(object oid, ISessionImplementor session, object owner)
		{
			//TODO: currently broken for unique-key references (does not detect
			//      change to unique key property of the associated object)
			object id = await (AssembleIdAsync(oid, session));
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
				return await (ResolveIdentifierAsync(id, session));
			}
		}

		public override async Task BeforeAssembleAsync(object oid, ISessionImplementor session)
		{
			ScheduleBatchLoadIfNeeded(await (AssembleIdAsync(oid, session)), session);
		}

		private async Task<object> AssembleIdAsync(object oid, ISessionImplementor session)
		{
			//the owner of the association is not the owner of the id
			return await (GetIdentifierType(session).AssembleAsync(oid, session, null));
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session)
		{
			if (IsSame(old, current, session.EntityMode))
			{
				return false;
			}

			object oldid = await (GetIdentifierAsync(old, session));
			object newid = await (GetIdentifierAsync(current, session));
			return await (GetIdentifierType(session).IsDirtyAsync(oldid, newid, session));
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			if (IsAlwaysDirtyChecked)
			{
				return await (IsDirtyAsync(old, current, session));
			}
			else
			{
				if (IsSame(old, current, session.EntityMode))
				{
					return false;
				}

				object oldid = await (GetIdentifierAsync(old, session));
				object newid = await (GetIdentifierAsync(current, session));
				return await (GetIdentifierType(session).IsDirtyAsync(oldid, newid, checkable, session));
			}
		}
	}
}
#endif
