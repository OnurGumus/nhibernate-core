#if NET_4_5
using System;
using NHibernate.Proxy;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1789
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class DomainObject : IDomainObject
	{
		/// <summary>
		/// Returns the concrete type of the object, not the proxy one.
		/// </summary>
		/// <returns></returns>
		public virtual Task<System.Type> GetConcreteTypeAsync()
		{
			return NHibernateProxyHelper.GuessClassAsync(this);
		}

		/// <summary>
		/// Turn a proxy object into a "real" object. If the <paramref name = "proxy"/> you give in parameter is not a INHibernateProxy, it will returns the same object without any change.
		/// </summary>
		/// <typeparam name = "T">Type in which the unproxied object should be returned</typeparam>
		/// <param name = "proxy">Proxy object</param>
		/// <returns>Unproxied object</returns>
		public static async Task<T> UnProxyAsync<T>(object proxy)
		{
			//If the object is not a proxy, just cast it and returns it
			if (!(proxy is INHibernateProxy))
			{
				return (T)proxy;
			}

			//Otherwise, use the NHibernate methods to get the implementation, and cast it
			var p = (INHibernateProxy)proxy;
			return (T)await (p.HibernateLazyInitializer.GetImplementationAsync());
		}

		/// <summary>
		/// Turn a proxy object into a "real" object. If the <paramref name = "proxy"/> you give in parameter is not a INHibernateProxy, it will returns the same object without any change.
		/// </summary>
		/// <param name = "proxy">Proxy object</param>
		/// <returns>Unproxied object</returns>
		public static Task<object> UnProxyAsync(object proxy)
		{
			return UnProxyAsync<object>(proxy);
		}
	}
}
#endif
