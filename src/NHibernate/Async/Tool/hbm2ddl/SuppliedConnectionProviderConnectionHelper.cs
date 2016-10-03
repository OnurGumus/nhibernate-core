#if NET_4_5
using System.Data.Common;
using NHibernate.Connection;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SuppliedConnectionProviderConnectionHelper : IConnectionHelper
	{
		public async Task PrepareAsync()
		{
			connection = (DbConnection)await (provider.GetConnectionAsync());
		}
	}
}
#endif
