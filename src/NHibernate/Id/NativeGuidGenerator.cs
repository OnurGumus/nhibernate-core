using System;
using System.Data;

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate.Driver;

namespace NHibernate.Id
{
	/// <summary>
	/// Generates Guid values using the server side Guid function.
	/// </summary>
	public class NativeGuidGenerator : IIdentifierGenerator
	{
		private static readonly IInternalLogger log = LoggerProvider.LoggerFor(typeof(NativeGuidGenerator));
		private readonly IType identifierType = new GuidType();

		#region Implementation of IIdentifierGenerator

		public async Task<object> Generate(ISessionImplementor session, object obj)
		{
			var sql = new SqlString(session.Factory.Dialect.SelectGUIDString);
			try
			{
				DbCommand st = await session.Batcher.PrepareCommand(CommandType.Text, sql, SqlTypeFactory.NoTypes).ConfigureAwait(false);
				IDataReaderEx reader = null;
				try
				{
					reader = await session.Batcher.ExecuteReader(st).ConfigureAwait(false);
					object result;
					try
					{
						await reader.ReadAsync().ConfigureAwait(false);
						result = await IdentifierGeneratorFactory.Get(reader, identifierType, session).ConfigureAwait(false);
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

		#endregion
	}
}