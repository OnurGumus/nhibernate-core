﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Data.Common;

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;

namespace NHibernate.Id.Enhanced
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class SequenceStructure : IDatabaseStructure
	{

		#region IDatabaseStructure Members

		#endregion

		#region Nested type: SequenceAccessCallback

		/// <content>
		/// Contains generated async methods
		/// </content>
		private partial class SequenceAccessCallback : IAccessCallback
		{

			#region IAccessCallback Members

			public virtual async Task<long> GetNextValueAsync(CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				_owner._accessCounter++;
				try
				{
					var st = await (_session.Batcher.PrepareCommandAsync(CommandType.Text, _owner._sql, SqlTypeFactory.NoTypes, cancellationToken)).ConfigureAwait(false);
					DbDataReader rs = null;
					try
					{
						rs = await (_session.Batcher.ExecuteReaderAsync(st, cancellationToken)).ConfigureAwait(false);
						try
						{
							await (rs.ReadAsync(cancellationToken)).ConfigureAwait(false);
							long result = Convert.ToInt64(rs.GetValue(0));
							if (Log.IsDebugEnabled)
							{
								Log.Debug("Sequence value obtained: " + result);
							}
							return result;
						}
						finally
						{
							try
							{
								rs.Close();
							}
							catch
							{
								// intentionally empty
							}
						}
					}
					finally
					{
						_session.Batcher.CloseCommand(st, rs);
					}
				}
				catch (DbException sqle)
				{
					throw ADOExceptionHelper.Convert(_session.Factory.SQLExceptionConverter, sqle, "could not get next sequence value", _owner._sql);
				}
			}

			#endregion
		}

		#endregion
	}
}