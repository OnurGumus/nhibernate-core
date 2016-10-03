#if NET_4_5
using System.Collections.Generic;
using NHibernate.Proxy.DynamicProxy;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.DynamicProxyTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InterfaceWithEqualsGethashcodeTests
	{
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InterceptedMethodsExposer : Proxy.DynamicProxy.IInterceptor
		{
			public Task<object> InterceptAsync(InvocationInfo info)
			{
				try
				{
					return Task.FromResult<object>(Intercept(info));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}
	}
}
#endif
