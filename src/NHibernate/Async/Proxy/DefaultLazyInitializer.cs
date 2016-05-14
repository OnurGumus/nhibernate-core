#if NET_4_5
using System;
using System.Reflection;
using NHibernate.Engine;
using NHibernate.Proxy.DynamicProxy;
using NHibernate.Proxy.Poco;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Proxy
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultLazyInitializer : BasicLazyInitializer, DynamicProxy.IInterceptor
	{
		public async Task<object> InterceptAsync(InvocationInfo info)
		{
			object returnValue;
			try
			{
				returnValue = await (base.InvokeAsync(info.TargetMethod, info.Arguments, info.Target));
				// Avoid invoking the actual implementation, if possible
				if (returnValue != InvokeImplementation)
				{
					return returnValue;
				}

				returnValue = info.TargetMethod.Invoke(await (GetImplementationAsync()), info.Arguments);
			}
			catch (TargetInvocationException ex)
			{
				throw ReflectHelper.UnwrapTargetInvocationException(ex);
			}

			return returnValue;
		}
	}
}
#endif
