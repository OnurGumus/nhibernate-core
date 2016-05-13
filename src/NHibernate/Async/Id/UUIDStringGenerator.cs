using System;
using System.Text;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that returns a string of length
	/// 16.  
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>&lt;generator class="uuid.string" /&gt;</code>
	/// </p>
	/// <para>
	/// The identifier string will NOT consist of only alphanumeric characters.  Use
	/// this only if you don't mind unreadable identifiers.
	/// </para>
	/// <para>
	/// This impelementation was known to be incompatible with Postgres.
	/// </para>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UUIDStringGenerator : IIdentifierGenerator
	{
		/// <summary>
		/// Generate a new <see cref = "String"/> for the identifier using the "uuid.string" algorithm.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "String"/>.</returns>
		public Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			try
			{
				return Task.FromResult<object>(Generate(session, obj));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}