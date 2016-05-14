#if NET_4_5
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Id.Enhanced
{
	/// <summary>
	/// Contract for providing callback access to an <see cref = "IDatabaseStructure"/>,
	/// typically from the <see cref = "IOptimizer"/>.
	/// </summary>
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
