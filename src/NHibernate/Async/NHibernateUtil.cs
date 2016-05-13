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
		/// <summary>
		/// Force initialization of a proxy or persistent collection.
		/// </summary>
		/// <param name = "proxy">a persistable object, proxy, persistent collection or null</param>
		/// <exception cref = "HibernateException">if we can't initialize the proxy at this time, eg. the Session was closed</exception>
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

		/// <summary>
		/// Get the true, underlying class of a proxied persistent class. This operation
		/// will initialize a proxy by side-effect.
		/// </summary>
		/// <param name = "proxy">a persistable object or proxy</param>
		/// <returns>the true class of the instance</returns>
		public static async Task<System.Type> GetClassAsync(object proxy)
		{
			if (proxy.IsProxy())
			{
				return (await (((INHibernateProxy)proxy).HibernateLazyInitializer.GetImplementationAsync())).GetType();
			}
			else
			{
				return proxy.GetType();
			}
		}

		/// <summary> 
		/// Check if the property is initialized. If the named property does not exist
		/// or is not persistent, this method always returns <tt>true</tt>. 
		/// </summary>
		/// <param name = "proxy">The potential proxy </param>
		/// <param name = "propertyName">the name of a persistent attribute of the object </param>
		/// <returns> 
		/// true if the named property of the object is not listed as uninitialized;
		/// false if the object is an uninitialized proxy, or the named property is uninitialized 
		/// </returns>
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