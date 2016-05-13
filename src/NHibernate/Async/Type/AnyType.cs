using System;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Xml;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class AnyType : AbstractType, IAbstractComponentType, IAssociationType
	{
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

		public override Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(NullSafeGet(rs, name, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override async Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return await (ResolveAnyAsync((string)await (metaType.NullSafeGetAsync(rs, names[0], session, owner)), await (identifierType.NullSafeGetAsync(rs, names[1], session, owner)), session));
		}

		public override async Task<object> HydrateAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			string entityName = (string)await (metaType.NullSafeGetAsync(rs, names[0], session, owner));
			object id = await (identifierType.NullSafeGetAsync(rs, names[1], session, owner));
			return new ObjectTypeCacheEntry(entityName, id);
		}

		public override async Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner)
		{
			ObjectTypeCacheEntry holder = (ObjectTypeCacheEntry)value;
			return await (ResolveAnyAsync(holder.entityName, holder.id, session));
		}

		public override Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(SemiResolve(value, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public override async Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			object id;
			string entityName;
			if (value == null)
			{
				id = null;
				entityName = null;
			}
			else
			{
				entityName = await (session.BestGuessEntityNameAsync(value));
				id = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(entityName, value, session));
			}

			// metaType is assumed to be single-column type
			if (settable == null || settable[0])
			{
				await (metaType.NullSafeSetAsync(st, entityName, index, session));
			}

			if (settable == null)
			{
				await (identifierType.NullSafeSetAsync(st, id, index + 1, session));
			}
			else
			{
				bool[] idsettable = new bool[settable.Length - 1];
				Array.Copy(settable, 1, idsettable, 0, idsettable.Length);
				await (identifierType.NullSafeSetAsync(st, id, index + 1, idsettable, session));
			}
		}

		public override Task NullSafeSetAsync(IDbCommand st, object value, int index, ISessionImplementor session)
		{
			return NullSafeSetAsync(st, value, index, null, session);
		}

		public override async Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			return value == null ? "null" : await (NHibernateUtil.Entity(NHibernateProxyHelper.GetClassWithoutInitializingProxy(value)).ToLoggableStringAsync(value, factory));
		}

		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			ObjectTypeCacheEntry e = cached as ObjectTypeCacheEntry;
			return (e == null) ? null : await (session.InternalLoadAsync(e.entityName, e.id, false, false));
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			return value == null ? null : new ObjectTypeCacheEntry(await (session.BestGuessEntityNameAsync(value)), await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(await (session.BestGuessEntityNameAsync(value)), value, session)));
		}

		public override async Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner, IDictionary copiedAlready)
		{
			if (original == null)
			{
				return null;
			}
			else
			{
				string entityName = await (session.BestGuessEntityNameAsync(original));
				object id = await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(entityName, original, session));
				return await (session.InternalLoadAsync(entityName, id, false, false));
			}
		}

		public async Task<object> GetPropertyValueAsync(Object component, int i, ISessionImplementor session)
		{
			return i == 0 ? await (session.BestGuessEntityNameAsync(component)) : await (IdAsync(component, session));
		}

		public Task<object[]> GetPropertyValuesAsync(Object component, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<object[]>(GetPropertyValues(component, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object[]>(ex);
			}
		}

		public async Task<object[]> GetPropertyValuesAsync(object component, ISessionImplementor session)
		{
			return new object[]{await (session.BestGuessEntityNameAsync(component)), await (IdAsync(component, session))};
		}

		private static async Task<object> IdAsync(object component, ISessionImplementor session)
		{
			try
			{
				return await (ForeignKeys.GetEntityIdentifierIfNotUnsavedAsync(await (session.BestGuessEntityNameAsync(component)), component, session));
			}
			catch (TransientObjectException)
			{
				return null;
			}
		}

		public override Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			//TODO!!!
			return IsDirtyAsync(old, current, session);
		}

		public override async Task<bool> IsModifiedAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			if (current == null)
				return old != null;
			if (old == null)
				return current != null;
			ObjectTypeCacheEntry holder = (ObjectTypeCacheEntry)old;
			bool[] idcheckable = new bool[checkable.Length - 1];
			Array.Copy(checkable, 1, idcheckable, 0, idcheckable.Length);
			return (checkable[0] && !holder.entityName.Equals(await (session.BestGuessEntityNameAsync(current)))) || await (identifierType.IsModifiedAsync(holder.id, await (IdAsync(current, session)), idcheckable, session));
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

		private Task<object> ResolveAnyAsync(string entityName, object id, ISessionImplementor session)
		{
			return entityName == null || id == null ? Task.FromResult<object>(null) : session.InternalLoadAsync(entityName, id, false, false);
		}

		public override Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping)
		{
			try
			{
				return Task.FromResult<bool[]>(ToColumnNullness(value, mapping));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool[]>(ex);
			}
		}
	}
}