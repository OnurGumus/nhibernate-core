using System.Collections;
using NHibernate.Criterion;
using NHibernate.Transform;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate
{
	/// <summary>
	/// Combines several queries into a single DB call
	/// </summary>
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