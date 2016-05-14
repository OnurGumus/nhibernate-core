#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Driver
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class BasicResultSetsCommand : IResultSetsCommand
	{
		public virtual async Task<DbDataReader> GetReaderAsync(int ? commandTimeout)
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

		protected virtual Task BindParametersAsync(DbCommand command)
		{
			try
			{
				BindParameters(command);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
