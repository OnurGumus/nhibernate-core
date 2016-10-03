#if NET_4_5
using System;
using NHibernate.Proxy.DynamicProxy;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.DynamicProxyTests
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PassThroughInterceptor : NHibernate.Proxy.DynamicProxy.IInterceptor
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
