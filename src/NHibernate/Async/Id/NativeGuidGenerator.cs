using System;
using System.Data;
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
				IDbCommand st = session.Batcher.PrepareCommand(CommandType.Text, sql, SqlTypeFactory.NoTypes);
				IDataReader reader = null;
				try
				{
					reader = await (session.Batcher.ExecuteReaderAsync(st));
					object result;
					try
					{
						reader.Read();
						result = IdentifierGeneratorFactory.Get(reader, identifierType, session);
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