using System;
using System.Data;

using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Data.Common;

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

		public object Generate(ISessionImplementor session, object obj)
		{
			var sql = new SqlString(session.Factory.Dialect.SelectGUIDString);
			try
			{
				DbCommand st = (DbCommand)session.Batcher.PrepareCommand(CommandType.Text, sql, SqlTypeFactory.NoTypes);
				IDataReader reader = null;
				try
				{
					try
					{
						reader = session.Batcher.ExecuteReader(st, false).Result;
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
					catch (AggregateException e)
					{
						throw e.InnerException;
					}
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