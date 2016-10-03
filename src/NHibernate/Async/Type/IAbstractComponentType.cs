#if NET_4_5
using System.Reflection;
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IAbstractComponentType : IType
	{
		/// <summary>
		/// Get the values of the component properties of 
		/// a component instance
		/// </summary>
		Task<object[]> GetPropertyValuesAsync(object component, ISessionImplementor session);
		Task<object> GetPropertyValueAsync(object component, int i, ISessionImplementor session);
	}
}
#endif
