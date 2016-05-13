using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Id.Enhanced
{
	/// <summary>
	/// Performs optimization on an optimizable identifier generator.  Typically
	/// this optimization takes the form of trying to ensure we do not have to
	/// hit the database on each and every request to get an identifier value.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Optimizers work on constructor injection.  They should provide
	/// a constructor with the following arguments.
	/// </para>
	/// - <see cref = "System.Type"/> The return type for the generated values.
	/// - <langword>int</langword> The increment size.
	/// </remarks>
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