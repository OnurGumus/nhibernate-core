using System;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that generates <see cref = "System.Guid"/> values 
	/// using a strategy suggested Jimmy Nilsson's 
	/// <a href = "http://www.informit.com/articles/article.asp?p=25862">article</a>
	/// on <a href = "http://www.informit.com">informit.com</a>. 
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>&lt;generator class="guid.comb" /&gt;</code>
	/// </p>
	/// <p>
	/// The <c>comb</c> algorithm is designed to make the use of GUIDs as Primary Keys, Foreign Keys, 
	/// and Indexes nearly as efficient as ints.
	/// </p>
	/// <p>
	/// This code was contributed by Donald Mull.
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GuidCombGenerator : IIdentifierGenerator
	{
		/// <summary>
		/// Generate a new <see cref = "Guid"/> using the comb algorithm.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "Guid"/>.</returns>
		public async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			return GenerateComb();
		}
	}
}