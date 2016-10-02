#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id.Enhanced
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IOptimizer
	{
		/// <summary>
		/// Generate an identifier value accounting for this specific optimization. 
		/// </summary>
		/// <param name = "callback">Callback to access the underlying value source. </param>
		/// <returns>The generated identifier value.</returns>
		Task<object> GenerateAsync(IAccessCallback callback);
	}
}
#endif
