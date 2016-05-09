using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Driver
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicResultSetsCommand : IResultSetsCommand
	{
		protected virtual async Task BindParametersAsync(IDbCommand command)
		{
			var wholeQueryParametersList = Sql.GetParameters().ToList();
			ForEachSqlCommand((sqlLoaderCommand, offset) => await (sqlLoaderCommand.BindAsync(command, wholeQueryParametersList, offset, Session)));
		}

		public virtual async Task<IDataReader> GetReaderAsync(int ? commandTimeout)
		{
			var batcher = Session.Batcher;
			SqlType[] sqlTypes = Commands.SelectMany(c => c.ParameterTypes).ToArray();
			ForEachSqlCommand((sqlLoaderCommand, offset) => sqlLoaderCommand.ResetParametersIndexesForTheCommand(offset));
			var command = batcher.PrepareQueryCommand(CommandType.Text, sqlString, sqlTypes);
			if (commandTimeout.HasValue)
			{
				command.CommandTimeout = commandTimeout.Value;
			}

			log.Info(command.CommandText);
			await (BindParametersAsync(command));
			return new BatcherDataReaderWrapper(batcher, command);
		}
	}
}