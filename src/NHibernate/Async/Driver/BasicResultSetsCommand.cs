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
	/// <summary>
	/// Datareader wrapper with the same life cycle of its command (through the batcher)
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BatcherDataReaderWrapper : IDataReader
	{
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicResultSetsCommand : IResultSetsCommand
	{
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
			BindParameters(command);
			return new BatcherDataReaderWrapperAsync(batcher, command);
		}
	}
}