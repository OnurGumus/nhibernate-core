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
		public virtual Task<IDataReader> ExecuteReaderAsync(IDbCommand cmd)
		{
			try
			{
				return Task.FromResult<IDataReader>(ExecuteReader(cmd));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IDataReader>(ex);
			}
		}
	}
}