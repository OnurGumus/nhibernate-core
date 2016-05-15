#if NET_4_5
using System.Data.Common;
using NHibernate.Connection;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	/// <summary>
	/// A <seealso cref = "IConnectionHelper"/> implementation based on a provided
	/// <seealso cref = "IConnectionProvider"/>.  Essentially, ensures that the connection
	/// gets cleaned up, but that the provider itself remains usable since it
	/// was externally provided to us.
	/// </summary>
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
