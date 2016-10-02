#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id.Enhanced
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IAccessCallback
	{
		/// <summary>
		/// Retrieve the next value from the underlying source.
		/// </summary>
		Task<long> GetNextValueAsync();
	}
}
#endif
