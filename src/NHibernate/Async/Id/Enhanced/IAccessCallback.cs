using System.Threading.Tasks;

namespace NHibernate.Id.Enhanced
{
	/// <summary>
	/// Contract for providing callback access to an <see cref = "IDatabaseStructure"/>,
	/// typically from the <see cref = "IOptimizer"/>.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IAccessCallback
	{
		Task<
		/// <summary>
		/// Retrieve the next value from the underlying source.
		/// </summary>
		long> GetNextValueAsync();
	}
}