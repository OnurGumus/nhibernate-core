#if NET_4_5
using System.Collections;
using NHibernate.Proxy.DynamicProxy;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.DynamicEntity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class DataProxyHandler : Proxy.DynamicProxy.IInterceptor
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
#endif
