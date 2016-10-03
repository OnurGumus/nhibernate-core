#if NET_4_5
using System.Data;
using System.Data.Common;
using NHibernate.Util;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace NHibernate.Driver
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class ReflectionBasedDriver : DriverBase
	{
		public override Task<DbConnection> CreateConnectionAsync()
		{
			try
			{
				return Task.FromResult<DbConnection>(CreateConnection());
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<DbConnection>(ex);
			}
		}
	}
}
#endif
