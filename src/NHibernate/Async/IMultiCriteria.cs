#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IMultiCriteria
	{
		/// <summary>
		/// Get all the results
		/// </summary>
		Task<IList> ListAsync();
		/// <summary>
		/// Returns the result of one of the Criteria based on the key
		/// </summary>
		/// <param name = "key">The key</param>
		/// <returns></returns>
		Task<object> GetResultAsync(string key);
	}
}
#endif
