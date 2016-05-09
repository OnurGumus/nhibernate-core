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
		public virtual async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			if (cached == null)
				return null;
			return await (DeepCopyAsync(cached, session.EntityMode, session.Factory));
		}

		public virtual async Task BeforeAssembleAsync(object cached, ISessionImplementor session)
		{
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
		public virtual async Task<object> ResolveIdentifierAsync(object value, ISessionImplementor session, object owner)
		{
			return value;
		}

		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(IDataReader, string[], ISessionImplementor, object)"]/*'
		////> 
		public abstract Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(IDataReader, string, ISessionImplementor, object)"]/*'
		////> 
		public abstract Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, Object owner);
		public virtual async Task<object> HydrateAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			return await (NullSafeGetAsync(rs, names, session, owner));
		}

		public abstract Task<object> ReplaceAsync(object original, object current, ISessionImplementor session, object owner, IDictionary copiedAlready);
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

		public virtual async Task<bool> IsModifiedAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return await (IsDirtyAsync(old, current, session));
		}

		public virtual async Task<bool> IsSameAsync(object x, object y, EntityMode entityMode)
		{
			return await (IsEqualAsync(x, y, entityMode));
		}

		public virtual async Task<bool> IsDirtyAsync(object old, object current, ISessionImplementor session)
		{
			return !await (IsSameAsync(old, current, session.EntityMode));
		}

		public abstract Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session);
		public virtual async Task<int> CompareAsync(object x, object y, EntityMode? entityMode)
		{
			IComparable xComp = x as IComparable;
			IComparable yComp = y as IComparable;
			if (xComp != null)
				return xComp.CompareTo(y);
			if (yComp != null)
				return yComp.CompareTo(x);
			throw new HibernateException(string.Format("Can't compare {0} with {1}; you must implement System.IComparable", x.GetType(), y.GetType()));
		}

		public virtual async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
		{
			return EqualsHelper.Equals(x, y);
		}

		public virtual async Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			return await (IsEqualAsync(x, y, entityMode));
		}

		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.DeepCopy"]/*'
		////> 
		public abstract Task<object> DeepCopyAsync(object val, EntityMode entityMode, ISessionFactoryImplementor factory);
		public virtual async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			if (value == null)
				return null;
			return await (DeepCopyAsync(value, session.EntityMode, session.Factory));
		}

		public abstract Task<bool[]> ToColumnNullnessAsync(object value, IMapping mapping);
		public virtual async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			return x.GetHashCode();
		}

		public virtual async Task<int> GetHashCodeAsync(object x, EntityMode entityMode, ISessionFactoryImplementor factory)
		{
			return await (GetHashCodeAsync(x, entityMode));
		}

		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.ToString"]/*'
		////> 
		public abstract Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeSet(settable)"]/*'
		////> 
		public abstract Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session);
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeSet"]/*'
		////> 
		public abstract Task NullSafeSetAsync(IDbCommand st, object value, int index, ISessionImplementor session);
		public virtual async Task<object> SemiResolveAsync(object value, ISessionImplementor session, object owner)
		{
			return value;
		}
	}
}