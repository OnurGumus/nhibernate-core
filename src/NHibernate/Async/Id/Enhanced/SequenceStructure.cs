#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Id.Enhanced
{
	/// <summary>
	/// Describes a sequence.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SequenceStructure : IDatabaseStructure
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		private partial class SequenceAccessCallback : IAccessCallback
		{
			public virtual async Task<long> GetNextValueAsync()
			{
				_owner._accessCounter++;
				try
				{
					DbCommand st = _session.Batcher.PrepareCommand(CommandType.Text, _owner._sql, SqlTypeFactory.NoTypes);
					DbDataReader rs = null;
					try
					{
						rs = await (_session.Batcher.ExecuteReaderAsync(st));
						try
						{
							rs.Read();
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
		}
	}
}
#endif
