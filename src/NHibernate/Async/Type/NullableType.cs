#if NET_4_5
using System;
using System.Data;
using System.Xml;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class NullableType : AbstractType
	{
		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.ToString"]/*'
		////> 
		/// <remarks>
		/// <para>
		/// This implementation forwards the call to <see cref = "ToString(object)"/> if the parameter 
		/// value is not null.
		/// </para>
		/// <para>
		/// It has been "sealed" because the Types inheriting from <see cref = "NullableType"/>
		/// do not need and should not override this method.  All of their implementation
		/// should be in <see cref = "ToString(object)"/>.
		/// </para>
		/// </remarks>
		public override sealed Task<string> ToLoggableStringAsync(object value, ISessionFactoryImplementor factory)
		{
			try
			{
				return Task.FromResult<string>(ToLoggableString(value, factory));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<string>(ex);
			}
		}

		public override Task NullSafeSetAsync(IDbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
		{
			try
			{
				NullSafeSet(st, value, index, settable, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeSet"]/*'
		////> 
		/// <remarks>
		/// <para>
		/// This implementation forwards the call to <see cref = "NullSafeSet(IDbCommand, object, int)"/>.
		/// </para>
		/// <para>
		/// It has been "sealed" because the Types inheriting from <see cref = "NullableType"/>
		/// do not need to and should not override this method.  All of their implementation
		/// should be in <see cref = "NullSafeSet(IDbCommand, object, int)"/>.
		/// </para>
		/// </remarks>
		public override sealed Task NullSafeSetAsync(IDbCommand st, object value, int index, ISessionImplementor session)
		{
			try
			{
				NullSafeSet(st, value, index, session);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(IDataReader, String[], ISessionImplementor, Object)"]/*'
		////> 
		/// <remarks>
		/// This has been sealed because no other class should override it.  This 
		/// method calls <see cref = "NullSafeGet(IDataReader, String)"/> for a single value.  
		/// It only takes the first name from the string[] names parameter - that is a 
		/// safe thing to do because a Nullable Type only has one field.
		/// </remarks>
		public override sealed Task<object> NullSafeGetAsync(IDataReader rs, string[] names, ISessionImplementor session, object owner)
		{
			try
			{
				return Task.FromResult<object>(NullSafeGet(rs, names, session, owner));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(IDataReader, String, ISessionImplementor, Object)"]/*'
		////> 
		/// <remarks>
		/// <para>
		/// This implementation forwards the call to <see cref = "NullSafeGet(IDataReader, String)"/>.
		/// </para>
		/// <para>
		/// It has been "sealed" because the Types inheriting from <see cref = "NullableType"/>
		/// do not need to and should not override this method.  All of their implementation
		/// should be in <see cref = "NullSafeGet(IDataReader, String)"/>.
		/// </para>
		/// </remarks>
		public override sealed Task<object> NullSafeGetAsync(IDataReader rs, string name, ISessionImplementor session, object owner)
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

		public override async Task<bool> IsDirtyAsync(object old, object current, bool[] checkable, ISessionImplementor session)
		{
			return checkable[0] && await (IsDirtyAsync(old, current, session));
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

		public override Task<bool> IsEqualAsync(object x, object y, EntityMode entityMode)
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
	}
}
#endif
