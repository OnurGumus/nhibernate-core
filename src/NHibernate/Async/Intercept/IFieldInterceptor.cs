#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Intercept
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IFieldInterceptor
	{
		/// <summary> Intercept field set/get </summary>
		Task<object> InterceptAsync(object target, string fieldName, object value);
	}
}
#endif
