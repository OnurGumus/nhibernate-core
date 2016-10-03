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
	public partial class OneToOneType : EntityType, IAssociationType
	{
		public override Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		//nothing to do
		}

		public override Task NullSafeSetAsync(DbCommand cmd, object value, int index, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		//nothing to do
		}

		public override Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<bool>(IsDirty(old, current, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public override Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<bool>(IsDirty(old, current, checkable, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public override Task<bool> IsModifiedAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<bool>(IsModified(old, current, checkable, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public override async Task<object> HydrateAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			IType type = GetIdentifierOrUniqueKeyType(session.Factory);
			object identifier = session.GetContextEntityIdentifier(owner);
			//This ugly mess is only used when mapping one-to-one entities with component ID types
			EmbeddedComponentType componentType = type as EmbeddedComponentType;
			if (componentType != null)
			{
				EmbeddedComponentType ownerIdType = session.GetEntityPersister(null, owner).IdentifierType as EmbeddedComponentType;
				if (ownerIdType != null)
				{
					object[] values = await (ownerIdType.GetPropertyValuesAsync(identifier, session));
					object id = await (componentType.ResolveIdentifierAsync(values, session, null));
					IEntityPersister persister = session.Factory.GetEntityPersister(type.ReturnedClass.FullName);
					var key = session.GenerateEntityKey(id, persister);
					return session.PersistenceContext.GetEntity(key);
				}
			}

			return identifier;
		}

		public override Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(Disassemble(value, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			//this should be a call to resolve(), not resolveIdentifier(), 
			//'cos it might be a property-ref, and we did not cache the
			//referenced value
			return await (ResolveIdentifierAsync(session.GetContextEntityIdentifier(owner), session, owner));
		}
	}
}
#endif
