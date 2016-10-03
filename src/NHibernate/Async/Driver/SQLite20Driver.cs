#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Driver
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SQLite20Driver : ReflectionBasedDriver
	{
		public override async Task<DbConnection> CreateConnectionAsync()
		{
			DbConnection connection = (DbConnection)await (base.CreateConnectionAsync());
			connection.StateChange += Connection_StateChange;
			return connection;
		}
	}
}
#endif
