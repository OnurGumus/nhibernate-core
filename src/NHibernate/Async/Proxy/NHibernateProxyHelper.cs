#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Intercept;
using System.Threading.Tasks;

namespace NHibernate.Proxy
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public static partial class NHibernateProxyHelper
	{
		/// <summary>
		/// Get the true, underlying class of a proxied persistent class. This operation
		/// will NOT initialize the proxy and thus may return an incorrect result.
		/// </summary>
		/// <param name = "entity">a persistable object or proxy</param>
		/// <returns>guessed class of the instance</returns>
		/// <remarks>
		/// This method is approximate match for Session.bestGuessEntityName in H3.2
		/// </remarks>
		public static async Task<System.Type> GuessClassAsync(object entity)
		{
			if (entity.IsProxy())
			{
				var proxy = entity as INHibernateProxy;
				var li = proxy.HibernateLazyInitializer;
				if (li.IsUninitialized)
				{
					return li.PersistentClass;
				}

				//NH-3145 : implementation could be a IFieldInterceptorAccessor 
				entity = await (li.GetImplementationAsync());
			}

			var fieldInterceptorAccessor = entity as IFieldInterceptorAccessor;
			if (fieldInterceptorAccessor != null)
			{
				var fieldInterceptor = fieldInterceptorAccessor.FieldInterceptor;
				return fieldInterceptor.MappedClass;
			}

			return entity.GetType();
		}
	}
}
#endif
