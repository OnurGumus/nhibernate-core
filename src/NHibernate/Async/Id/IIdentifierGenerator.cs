#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IIdentifierGenerator
	{
		/// <summary>
		/// Generate a new identifier
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier</returns>
		Task<object> GenerateAsync(ISessionImplementor session, object obj);
	}
}
#endif
