#if NET_4_5
using System.Data.Common;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id.Insert
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IBinder
	{
		Task BindValuesAsync(DbCommand cm);
	}
}
#endif
