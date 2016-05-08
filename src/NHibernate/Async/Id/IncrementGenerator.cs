using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <c>IIdentifierGenerator</c> that returns a <c>Int64</c>, constructed by
	/// counting from the maximum primary key value at startup. Not safe for use in a
	/// cluster!
	/// </summary>
	/// <remarks>
	/// <para>
	/// java author Gavin King, .NET port Mark Holden
	/// </para>
	/// <para>
	/// Mapping parameters supported, but not usually needed: table, column.
	/// </para>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IncrementGenerator : IIdentifierGenerator, IConfigurable
	{
		private async Task GetNextAsync(ISessionImplementor session)
		{
			Logger.Debug("fetching initial value: " + _sql);
			try
			{
				var cmd = session.Batcher.PrepareCommand(CommandType.Text, _sql, SqlTypeFactory.NoTypes);
				IDataReader reader = null;
				try
				{
					reader = await (session.Batcher.ExecuteReaderAsync(cmd));
					try
					{
						if (reader.Read())
						{
							_next = !reader.IsDBNull(0) ? Convert.ToInt64(reader.GetValue(0)) + 1 : 1L;
						}
						else
						{
							_next = 1L;
						}

						_sql = null;
						Logger.Debug("first free id: " + _next);
					}
					finally
					{
						reader.Close();
					}
				}
				finally
				{
					session.Batcher.CloseCommand(cmd, reader);
				}
			}
			catch (DbException sqle)
			{
				Logger.Error("could not get increment value", sqle);
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not fetch initial value for increment generator");
			}
		}

		[MethodImpl(MethodImplOptions.Synchronized)]
		public async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			if (_sql != null)
			{
				await (GetNextAsync(session));
			}

			return IdentifierGeneratorFactory.CreateNumber(_next++, _returnClass);
		}
	}
}