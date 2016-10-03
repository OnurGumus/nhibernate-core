#if NET_4_5
using System;
using NHibernate.Proxy.DynamicProxy;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Intercept
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DefaultDynamicLazyFieldInterceptor : IFieldInterceptorAccessor, Proxy.DynamicProxy.IInterceptor
	{
		public async Task<object> InterceptAsync(InvocationInfo info)
		{
			var methodName = info.TargetMethod.Name;
			if (FieldInterceptor != null)
			{
				if (ReflectHelper.IsPropertyGet(info.TargetMethod))
				{
					if ("get_FieldInterceptor".Equals(methodName))
					{
						return FieldInterceptor;
					}

					object propValue = info.InvokeMethodOnTarget();
					var result = await (FieldInterceptor.InterceptAsync(info.Target, ReflectHelper.GetPropertyName(info.TargetMethod), propValue));
					if (result != AbstractFieldInterceptor.InvokeImplementation)
					{
						return result;
					}
				}
				else if (ReflectHelper.IsPropertySet(info.TargetMethod))
				{
					if ("set_FieldInterceptor".Equals(methodName))
					{
						FieldInterceptor = (IFieldInterceptor)info.Arguments[0];
						return null;
					}

					FieldInterceptor.MarkDirty();
					await (FieldInterceptor.InterceptAsync(info.Target, ReflectHelper.GetPropertyName(info.TargetMethod), info.Arguments[0]));
				}
			}
			else
			{
				if ("set_FieldInterceptor".Equals(methodName))
				{
					FieldInterceptor = (IFieldInterceptor)info.Arguments[0];
					return null;
				}
			}

			return info.InvokeMethodOnTarget();
		}
	}
}
#endif
