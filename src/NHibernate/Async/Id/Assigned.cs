#if NET_4_5
using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Assigned : IIdentifierGenerator, IConfigurable
	{
		/// <summary>
		/// Generates a new identifier by getting the value of the identifier
		/// for the <c>obj</c> parameter.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The value that was assigned to the mapped <c>id</c>'s property.</returns>
		/// <exception cref = "IdentifierGenerationException">
		/// Thrown when a <see cref = "IPersistentCollection"/> is passed in as the <c>obj</c> or
		/// if the identifier of <c>obj</c> is null.
		/// </exception>
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
#endif
