using System;
using System.Collections;
using System.Data;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractType : IType
	{
		/// <summary>
		/// Disassembles the object into a cacheable representation.
		/// </summary>
		/// <param name = "value">The value to disassemble.</param>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> is not used by this method.</param>
		/// <param name = "owner">optional parent entity object (needed for collections) </param>
		/// <returns>The disassembled, deep cloned state of the object</returns>
		/// <remarks>
		/// This method calls DeepCopy if the value is not null.
		/// </remarks>
		public virtual async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			if (value == null)
				return null;
			return await (DeepCopyAsync(value, session.EntityMode, session.Factory));
		}

		/// <summary>
		/// Reconstructs the object from its cached "disassembled" state.
		/// </summary>
		/// <param name = "cached">The disassembled state from the cache</param>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> is not used by this method.</param>
		/// <param name = "owner">The parent Entity object is not used by this method</param>
		/// <returns>The assembled object.</returns>
		/// <remarks>
		/// This method calls DeepCopy if the value is not null.
		/// </remarks>
		public virtual async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			if (cached == null)
				return null;
			return await (DeepCopyAsync(cached, session.EntityMode, session.Factory));
		}

		public virtual Task BeforeAssembleAsync(object cached, ISessionImplementor session)
		{
			return TaskHelper.CompletedTask;
		}

		/// <summary>
		/// Should the parent be considered dirty, given both the old and current 
		/// field or element value?
		/// </summary>
		/// <param name = "old">The old value</param>
		/// <param name = "current">The current value</param>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> is not used by this method.</param>
		/// <returns>true if the field is dirty</returns>
		/// <remarks>This method uses <c>IType.Equals(object, object)</c> to determine the value of IsDirty.</remarks>
		public virtual async Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session)
		{
			return !await (IsSameAsync(old, current, session.EntityMode));
		}

		/// <summary>
		/// Retrieves an instance of the mapped class, or the identifier of an entity 
		/// or collection from a <see cref = "IDataReader"/>.
		/// </summary>
		/// <param name = "rs">The <see cref = "IDataReader"/> that contains the values.</param>
		/// <param name = "names">
		/// The names of the columns in the <see cref = "IDataReader"/> that contain the 
		/// value to populate the IType with.
		/// </param>
		/// <param name = "session">the session</param>
		/// <param name = "owner">The parent Entity</param>
		/// <returns>An identifier or actual object mapped by this IType.</returns>
		/// <remarks>
		/// This method uses the <c>IType.NullSafeGet(IDataReader, string[], ISessionImplementor, object)</c> method
		/// to Hydrate this <see cref = "AbstractType"/>.
		/// </remarks>
		public virtual Task<object> HydrateAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return NullSafeGetAsync(rs, names, session, owner);
		}

		/// <summary>
		/// Maps identifiers to Entities or Collections. 
		/// </summary>
		/// <param name = "value">An identifier or value returned by <c>Hydrate()</c></param>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> is not used by this method.</param>
		/// <param name = "owner">The parent Entity is not used by this method.</param>
		/// <returns>The value.</returns>
		/// <remarks>
		/// There is nothing done in this method other than return the value parameter passed in.
		/// </remarks>
		public virtual Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner)
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

		public virtual Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner)
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

		/// <summary>
		/// Says whether the value has been modified
		/// </summary>
		public virtual Task<bool> IsModifiedAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return IsDirtyAsync(old, current, session);
		}

		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.DeepCopy"]/*'
		////> 
		public abstract Task<object> DeepCopyAsync(object val, EntityMode entityMode, ISessionFactoryImplementor factory);
		public virtual async Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner, IDictionary copyCache, ForeignKeyDirection foreignKeyDirection)
		{
			bool include;
			if (IsAssociationType)
			{
				IAssociationType atype = (IAssociationType)this;
				include = atype.ForeignKeyDirection == foreignKeyDirection;
			}
			else
			{
				include = ForeignKeyDirection.ForeignKeyFromParent.Equals(foreignKeyDirection);
			}

			return include ? await (ReplaceAsync(original, target, session, owner, copyCache)) : target;
		}

		public virtual Task<bool> IsSameAsync(object x, object y, EntityMode entityMode)
		{
			return IsEqualAsync(x, y, entityMode);
		}

		public virtual Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<bool>(IsEqual(x, y, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<bool>(ex);
			}
		}

		public virtual Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			return IsEqualAsync(x, y, entityMode);
		}

		public virtual Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			try
			{
				return Task.FromResult<int>(GetHashCode(x, entityMode));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<int>(ex);
			}
		}

		public virtual Task<int> GetHashCodeAsync(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			return GetHashCodeAsync(x, entityMode);
		}

		public virtual Task<int> CompareAsync(object x, object y, EntityMode? entityMode)
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

		public abstract Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner, IDictionary copiedAlready);
		public abstract Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(IDataReader, string[], ISessionImplementor, object)"]/*'
		////> 
		public abstract Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(IDataReader, string, ISessionImplementor, object)"]/*'
		////> 
		public abstract Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, Object owner);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeSet(settable)"]/*'
		////> 
		public abstract Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeSet"]/*'
		////> 
		public abstract Task NullSafeSetAsync(IDbCommand st, object value, int index, ISessionImplementor session);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.ToString"]/*'
		////> 
		public abstract Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory);
		public abstract Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session);
	}
}