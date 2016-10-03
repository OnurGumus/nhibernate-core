#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.AdoNet
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ConnectionManager : ISerializable, IDeserializationCallback
	{
		public async Task<DbConnection> GetConnectionAsync()
		{
			if (connection == null)
			{
				if (ownConnection)
				{
					connection = await (Factory.ConnectionProvider.GetConnectionAsync());
					if (Factory.Statistics.IsStatisticsEnabled)
					{
						Factory.StatisticsImplementor.Connect();
					}
				}
				else if (session.IsOpen)
				{
					throw new HibernateException("Session is currently disconnected");
				}
				else
				{
					throw new HibernateException("Session is closed");
				}
			}

			return connection;
		}

		public async Task<DbCommand> CreateCommandAsync()
		{
			var result = (await (GetConnectionAsync())).CreateCommand();
			Transaction.Enlist(result);
			return result;
		}
	}
}
#endif
