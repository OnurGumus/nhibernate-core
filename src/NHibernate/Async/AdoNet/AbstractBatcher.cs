#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
		public virtual Task<DbDataReader> ExecuteReaderAsync(DbCommand cmd)
		{
			try
			{
				return Task.FromResult<DbDataReader>(ExecuteReader(cmd));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<DbDataReader>(ex);
			}
		}
	}
}
#endif
