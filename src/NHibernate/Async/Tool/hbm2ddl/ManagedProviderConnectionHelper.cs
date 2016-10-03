#if NET_4_5
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Connection;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ManagedProviderConnectionHelper : IConnectionHelper
	{
		public async Task PrepareAsync()
		{
			connectionProvider = ConnectionProviderFactory.NewConnectionProvider(cfgProperties);
			connection = (DbConnection)await (connectionProvider.GetConnectionAsync());
		}
	}
}
#endif
