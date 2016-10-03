#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1789
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IDomainObject
	{
		/// <summary>
		/// Returns the concrete type of the object, not the proxy one.
		/// </summary>
		/// <returns></returns>
		Task<System.Type> GetConcreteTypeAsync();
	}
}
#endif
