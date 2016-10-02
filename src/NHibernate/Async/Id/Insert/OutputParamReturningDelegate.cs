#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Id.Insert
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class OutputParamReturningDelegate : AbstractReturningDelegate
	{
		protected internal override async Task<DbCommand> PrepareAsync(SqlCommandInfo insertSQL, ISessionImplementor session)
		{
			DbCommand command = await (session.Batcher.PrepareCommandAsync(CommandType.Text, insertSQL.Text, insertSQL.ParameterTypes));
			//Add the output parameter
			DbParameter idParameter = factory.ConnectionProvider.Driver.GenerateParameter(command, ReturnParameterName, paramType);
			driveGeneratedParamName = idParameter.ParameterName;
			if (factory.Dialect.InsertGeneratedIdentifierRetrievalMethod == InsertGeneratedIdentifierRetrievalMethod.OutputParameter)
				idParameter.Direction = ParameterDirection.Output;
			else if (factory.Dialect.InsertGeneratedIdentifierRetrievalMethod == InsertGeneratedIdentifierRetrievalMethod.ReturnValueParameter)
				idParameter.Direction = ParameterDirection.ReturnValue;
			else
				throw new System.NotImplementedException("Unsupported InsertGeneratedIdentifierRetrievalMethod: " + factory.Dialect.InsertGeneratedIdentifierRetrievalMethod);
			command.Parameters.Add(idParameter);
			return command;
		}

		public override async Task<object> ExecuteAndExtractAsync(DbCommand insert, ISessionImplementor session)
		{
			await (session.Batcher.ExecuteNonQueryAsync(insert));
			return Convert.ChangeType(((DbParameter)insert.Parameters[driveGeneratedParamName]).Value, Persister.IdentifierType.ReturnedClass);
		}
	}
}
#endif
