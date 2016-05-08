using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Exceptions;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Util;
using NHibernate.AdoNet.Util;
using System.Threading.Tasks;

namespace NHibernate.AdoNet
{
	/// <summary>
	/// Manages prepared statements and batching. Class exists to enforce separation of concerns
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractBatcher : IBatcher
	{
		public virtual async Task<IDataReader> ExecuteReaderAsync(IDbCommand cmd)
		{
			CheckReaders();
			LogCommand(cmd);
			Prepare(cmd);
			Stopwatch duration = null;
			if (Log.IsDebugEnabled)
				duration = Stopwatch.StartNew();
			IDataReader reader = null;
			try
			{
				reader = cmd.ExecuteReader();
			}
			catch (Exception e)
			{
				e.Data["actual-sql-query"] = cmd.CommandText;
				Log.Error("Could not execute query: " + cmd.CommandText, e);
				throw;
			}
			finally
			{
				if (Log.IsDebugEnabled && duration != null && reader != null)
				{
					Log.DebugFormat("ExecuteReader took {0} ms", duration.ElapsedMilliseconds);
					_readersDuration[reader] = duration;
				}
			}

			if (!_factory.ConnectionProvider.Driver.SupportsMultipleOpenReaders)
			{
				reader = new NHybridDataReader(reader);
			}

			_readersToClose.Add(reader);
			LogOpenReader();
			return reader;
		}
	}
}