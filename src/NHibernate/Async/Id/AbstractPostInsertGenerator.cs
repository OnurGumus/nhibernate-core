#if NET_4_5
using NHibernate.Engine;
using NHibernate.Id.Insert;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractPostInsertGenerator : IPostInsertIdentifierGenerator
	{
		/// <summary>
		/// The IdentityGenerator for autoincrement/identity key generation. 
		/// </summary>
		/// <param name = "s">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity the id is being generated for.</param>
		/// <returns>
		/// <c>IdentityColumnIndicator</c> Indicates to the Session that identity (i.e. identity/autoincrement column)
		/// key generation should be used.
		/// </returns>
		public Task<object> GenerateAsync(ISessionImplementor s, object obj)
		{
			try
			{
				return Task.FromResult<object>(Generate(s, obj));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
