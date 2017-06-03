﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Util;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public abstract partial class NullableType : AbstractType
	{

		/// <inheritdoc />
		/// <remarks>
		/// This has been sealed because no other class should override it.  This 
		/// method calls <see cref="NullSafeGetAsync(DbDataReader, String, ISessionImplementor,CancellationToken)" /> for a single value.  
		/// It only takes the first name from the string[] names parameter - that is a 
		/// safe thing to do because a Nullable Type only has one field.
		/// </remarks>
		public sealed override Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			return NullSafeGetAsync(rs, names[0], session, cancellationToken);
		}

		/// <summary>
		/// Extracts the values of the fields from the DataReader
		/// </summary>
		/// <param name="rs">The DataReader positioned on the correct record</param>
		/// <param name="names">An array of field names.</param>
		/// <param name="session">The session for which the operation is done.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns>The value off the field from the DataReader</returns>
		/// <remarks>
		/// In this class this just ends up passing the first name to the NullSafeGet method
		/// that takes a string, not a string[].
		/// 
		/// I don't know why this method is in here - it doesn't look like anybody that inherits
		/// from NullableType overrides this...
		/// 
		/// TODO: determine if this is needed
		/// </remarks>
		public virtual Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, ISessionImplementor session, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			return NullSafeGetAsync(rs, names[0], session, cancellationToken);
		}

		/// <summary>
		/// Gets the value of the field from the <see cref="DbDataReader" />.
		/// </summary>
		/// <param name="rs">The <see cref="DbDataReader" /> positioned on the correct record.</param>
		/// <param name="name">The name of the field to get the value from.</param>
		/// <param name="session">The session for which the operation is done.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		/// <returns>The value of the field.</returns>
		/// <remarks>
		/// <para>
		/// This method checks to see if value is null, if it is then the null is returned
		/// from this method.
		/// </para>
		/// <para>
		/// If the value is not null, then the method <see cref="Get(DbDataReader, Int32, ISessionImplementor)"/> 
		/// is called and that method is responsible for retrieving the value.
		/// </para>
		/// </remarks>
		public virtual async Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			int index = rs.GetOrdinal(name);

			if (await (rs.IsDBNullAsync(index, cancellationToken)).ConfigureAwait(false))
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
					val = Get(rs, index, session);
				}
				catch (InvalidCastException ice)
				{
					throw new ADOException(
						string.Format(
							"Could not cast the value in field {0} of type {1} to the Type {2}.  Please check to make sure that the mapping is correct and that your DataProvider supports this Data Type.",
							name, rs[index].GetType().Name, GetType().Name), ice);
				}

				if (IsDebugEnabled)
				{
					Log.Debug("returning '" + ToString(val) + "' as column: " + name);
				}

				return val;
			}
		}

		/// <inheritdoc />
		/// <remarks>
		/// <para>
		/// This implementation forwards the call to <see cref="NullSafeGetAsync(DbDataReader, String, ISessionImplementor,CancellationToken)" />.
		/// </para>
		/// <para>
		/// It has been "sealed" because the Types inheriting from <see cref="NullableType"/>
		/// do not need to and should not override this method.  All of their implementation
		/// should be in <see cref="NullSafeGetAsync(DbDataReader, String, ISessionImplementor,CancellationToken)" />.
		/// </para>
		/// </remarks>
		public sealed override Task<object> NullSafeGetAsync(DbDataReader rs, string name, ISessionImplementor session, object owner, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			return NullSafeGetAsync(rs, name, session, cancellationToken);
		}

		#region override of System.Object Members

		#endregion
	}
}
