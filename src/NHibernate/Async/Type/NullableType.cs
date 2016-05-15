#if NET_4_5
using System;
using System.Data.Common;
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

		public override Task NullSafeSetAsync(DbCommand st, object value, int index, bool[] settable, ISessionImplementor session)
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
		/// This implementation forwards the call to <see cref = "NullSafeSet(DbCommand, object, int)"/>.
		/// </para>
		/// <para>
		/// It has been "sealed" because the Types inheriting from <see cref = "NullableType"/>
		/// do not need to and should not override this method.  All of their implementation
		/// should be in <see cref = "NullSafeSet(DbCommand, object, int)"/>.
		/// </para>
		/// </remarks>
		public override sealed Task NullSafeSetAsync(DbCommand st, object value, int index, ISessionImplementor session)
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
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(DbDataReader, String[], ISessionImplementor, Object)"]/*'
		////> 
		/// <remarks>
		/// This has been sealed because no other class should override it.  This 
		/// method calls <see cref = "NullSafeGet(DbDataReader, String)"/> for a single value.  
		/// It only takes the first name from the string[] names parameter - that is a 
		/// safe thing to do because a Nullable Type only has one field.
		/// </remarks>
		public override sealed Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
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

		/// <summary>
		/// Gets the value of the field from the <see cref = "DbDataReader"/>.
		/// </summary>
		/// <param name = "rs">The <see cref = "DbDataReader"/> positioned on the correct record.</param>
		/// <param name = "name">The name of the field to get the value from.</param>
		/// <returns>The value of the field.</returns>
		/// <remarks>
		/// <para>
		/// This method checks to see if value is null, if it is then the null is returned
		/// from this method.
		/// </para>
		/// <para>
		/// If the value is not null, then the method <see cref = "Get(DbDataReader, Int32)"/> 
		/// is called and that method is responsible for retrieving the value.
		/// </para>
		/// </remarks>
		public virtual async Task<object> NullSafeGetAsync(DbDataReader rs, string name)
		{
			int index = rs.GetOrdinal(name);
			if (await (rs.IsDBNullAsync(index)))
			{
				if (IsDebugEnabled)
				{
					Log.Debug("returning null as column: " + name);
				}

				// TODO: add a method to NullableType.GetNullValue - if we want to
				// use "MAGIC" numbers to indicate null values...
				return null;
			}
			else
			{
				object val;
				try
				{
					val = Get(rs, index);
				}
				catch (InvalidCastException ice)
				{
					throw new ADOException(string.Format("Could not cast the value in field {0} of type {1} to the Type {2}.  Please check to make sure that the mapping is correct and that your DataProvider supports this Data Type.", name, rs[index].GetType().Name, GetType().Name), ice);
				}

				if (IsDebugEnabled)
				{
					Log.Debug("returning '" + ToString(val) + "' as column: " + name);
				}

				return val;
			}
		}

		/// <include file = 'IType.cs.xmldoc'
		///path = '//members[@type="IType"]/member[@name="M:IType.NullSafeGet(DbDataReader, String, ISessionImplementor, Object)"]/*'
		////> 
		/// <remarks>
		/// <para>
		/// This implementation forwards the call to <see cref = "NullSafeGet(DbDataReader, String)"/>.
		/// </para>
		/// <para>
		/// It has been "sealed" because the Types inheriting from <see cref = "NullableType"/>
		/// do not need to and should not override this method.  All of their implementation
		/// should be in <see cref = "NullSafeGet(DbDataReader, String)"/>.
		/// </para>
		/// </remarks>
		public override sealed Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner)
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
