using System;
using System.Collections;
using NHibernate.Collection;
using NHibernate.Impl;
using NHibernate.Intercept;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.UserTypes;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate
{
	using System.Collections.Generic;
	using System.Reflection;

	/// <summary>
	/// Provides access to the full range of NHibernate built-in types.
	/// IType instances may be used to bind values to query parameters.
	/// Also a factory for new Blobs and Clobs.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class NHibernateUtil
	{
		public static async Task InitializeAsync(object proxy)
		{
			if (proxy == null)
			{
				return;
			}
			else if (proxy.IsProxy())
			{
				await (((INHibernateProxy)proxy).HibernateLazyInitializer.InitializeAsync());
			}
			else if (proxy is IPersistentCollection)
			{
				await (((IPersistentCollection)proxy).ForceInitializationAsync());
			}
		}

		public static async Task<System.Type> GetClassAsync(object proxy)
		{
			if (proxy.IsProxy())
			{
				return await (((INHibernateProxy)proxy).HibernateLazyInitializer.GetImplementationAsync()).GetType();
			}
			else
			{
				return proxy.GetType();
			}
		}

		public static async Task<bool> IsPropertyInitializedAsync(object proxy, string propertyName)
		{
			object entity;
			if (proxy.IsProxy())
			{
				ILazyInitializer li = ((INHibernateProxy)proxy).HibernateLazyInitializer;
				if (li.IsUninitialized)
				{
					return false;
				}
				else
				{
					entity = await (li.GetImplementationAsync());
				}
			}
			else
			{
				entity = proxy;
			}

			if (FieldInterceptionHelper.IsInstrumented(entity))
			{
				IFieldInterceptor interceptor = FieldInterceptionHelper.ExtractFieldInterceptor(entity);
				return interceptor == null || interceptor.IsInitializedField(propertyName);
			}
			else
			{
				return true;
			}
		}
	}
}