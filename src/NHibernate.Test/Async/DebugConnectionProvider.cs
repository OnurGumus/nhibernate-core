﻿#if NET_4_5
using System;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.CompilerServices;
using NHibernate.Connection;
using System.Threading.Tasks;

namespace NHibernate.Test
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DebugConnectionProvider : DriverConnectionProvider
	{
		public override async Task<DbConnection> GetConnectionAsync()
		{
			try
			{
				DbConnection connection = await (base.GetConnectionAsync());
				lock (lockObject)
				{
					connections.Add(connection);
				}
				return connection;
			}
			catch (Exception e)
			{
				throw new HibernateException("Could not open connection to: " + ConnectionString, e);
			}
		}
	}
}
#endif
