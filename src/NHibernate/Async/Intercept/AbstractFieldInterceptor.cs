#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Proxy;
using System.Threading.Tasks;

namespace NHibernate.Intercept
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractFieldInterceptor : IFieldInterceptor
	{
		public async Task<object> InterceptAsync(object target, string fieldName, object value)
		{
			// NH Specific: Hibernate only deals with lazy properties here, we deal with 
			// both lazy properties and with no-proxy. 
			if (initializing)
			{
				return InvokeImplementation;
			}

			if (IsInitializedField(fieldName))
			{
				if (value.IsProxy() && IsInitializedAssociation(fieldName))
					return await (InitializeOrGetAssociationAsync((INHibernateProxy)value, fieldName));
				return value;
			}

			if (session == null)
			{
				throw new LazyInitializationException(EntityName, null, string.Format("entity with lazy properties is not associated with a session. entity-name:'{0}' property:'{1}'", EntityName, fieldName));
			}

			if (!session.IsOpen || !session.IsConnected)
			{
				throw new LazyInitializationException(EntityName, null, string.Format("session is not connected. entity-name:'{0}' property:'{1}'", EntityName, fieldName));
			}

			if (IsUninitializedProperty(fieldName))
			{
				return await (InitializeFieldAsync(fieldName, target));
			}

			if (value.IsProxy() && IsUninitializedAssociation(fieldName))
			{
				var nhproxy = value as INHibernateProxy;
				return await (InitializeOrGetAssociationAsync(nhproxy, fieldName));
			}

			return InvokeImplementation;
		}

		private async Task<object> InitializeOrGetAssociationAsync(INHibernateProxy value, string fieldName)
		{
			if (value.HibernateLazyInitializer.IsUninitialized)
			{
				await (value.HibernateLazyInitializer.InitializeAsync());
				value.HibernateLazyInitializer.Unwrap = true; // means that future Load/Get from the session will get the implementation
				loadedUnwrapProxyFieldNames.Add(fieldName);
			}

			return value.HibernateLazyInitializer.GetImplementation(session);
		}

		private async Task<object> InitializeFieldAsync(string fieldName, object target)
		{
			object result;
			initializing = true;
			try
			{
				var lazyPropertyInitializer = ((ILazyPropertyInitializer)session.Factory.GetEntityPersister(entityName));
				result = await (lazyPropertyInitializer.InitializeLazyPropertyAsync(fieldName, target, session));
			}
			finally
			{
				initializing = false;
			}

			uninitializedFields = null; //let's assume that there is only one lazy fetch group, for now!
			return result;
		}
	}
}
#endif
