#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// Generates Guid values using the server side Guid function.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NativeGuidGenerator : IIdentifierGenerator
	{
		public async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			var sql = new SqlString(session.Factory.Dialect.SelectGUIDString);
			try
			{
				DbCommand st = await (session.Batcher.PrepareCommandAsync(CommandType.Text, sql, SqlTypeFactory.NoTypes));
				DbDataReader reader = null;
				try
				{
					reader = await (session.Batcher.ExecuteReaderAsync(st));
					object result;
					try
					{
						await (reader.ReadAsync());
						result = await (IdentifierGeneratorFactory.GetAsync(reader, identifierType, session));
					}
					finally
					{
						reader.Close();
					}

					log.Debug("GUID identifier generated: " + result);
					return result;
				}
				finally
				{
					session.Batcher.CloseCommand(st, reader);
				}
			}
			catch (Exception sqle)
			{
				throw ADOExceptionHelper.Convert(session.Factory.SQLExceptionConverter, sqle, "could not retrieve GUID", sql);
			}
		}
	}
}
#endif
