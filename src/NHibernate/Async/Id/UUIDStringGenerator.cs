#if NET_4_5
using System;
using System.Text;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Id
{
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
#endif
