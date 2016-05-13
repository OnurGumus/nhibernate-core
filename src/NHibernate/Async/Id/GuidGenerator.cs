using System;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that generates <see cref = "Guid"/> values 
	/// using <see cref = "System.Guid.NewGuid()">Guid.NewGuid()</see>. 
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>&lt;generator class="guid" /&gt;</code>
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class GuidGenerator : IIdentifierGenerator
	{
		/// <summary>
		/// Generate a new <see cref = "Guid"/> for the identifier.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "Guid"/>.</returns>
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