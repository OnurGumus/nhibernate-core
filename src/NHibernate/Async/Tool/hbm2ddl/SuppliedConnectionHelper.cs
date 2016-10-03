#if NET_4_5
using System.Data.Common;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Tool.hbm2ddl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SuppliedConnectionHelper : IConnectionHelper
	{
		public Task PrepareAsync()
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
