using System;
using System.Data;
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
		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			//this should be a call to resolve(), not resolveIdentifier(), 
			//'cos it might be a property-ref, and we did not cache the
			//referenced value
			return await (ResolveIdentifierAsync(session.GetContextEntityIdentifier(owner), session, owner));
		}

		public override async Task<object> HydrateAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
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

		public override async Task<bool> IsModifiedAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return false;
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session)
		{
			return false;
		}

		public override async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return false;
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			return null;
		}

		public override async Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
		{
			return ArrayHelper.EmptyBoolArray;
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
		//nothing to do
		}

		public override async Task NullSafeSetAsync(IDbCommand cmd, object value, int index, ISessionImplementor session)
		{
		//nothing to do
		}
	}
}