using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Proxy.Poco
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class BasicLazyInitializer : AbstractLazyInitializer
	{
		public virtual async Task<object> InvokeAsync(MethodInfo method, object[] args, object proxy)
		{
			string methodName = method.Name;
			int paramCount = method.GetParameters().Length;
			if (paramCount == 0)
			{
				if (!overridesEquals && methodName == "GetHashCode")
				{
					return IdentityEqualityComparer.GetHashCode(proxy);
				}
				else if (IsEqualToIdentifierMethod(method))
				{
					return Identifier;
				}
				else if (methodName == "Dispose")
				{
					return null;
				}
				else if ("get_HibernateLazyInitializer".Equals(methodName))
				{
					return this;
				}
			}
			else if (paramCount == 1)
			{
				if (!overridesEquals && methodName == "Equals")
				{
					return IdentityEqualityComparer.Equals(args[0], proxy);
				}
				else if (setIdentifierMethod != null && method.Equals(setIdentifierMethod))
				{
					await (InitializeAsync());
					Identifier = args[0];
					return InvokeImplementation;
				}
			}
			else if (paramCount == 2)
			{
				// if the Proxy Engine delegates the call of GetObjectData to the Initializer
				// then we need to handle it.  Castle.DynamicProxy takes care of serializing
				// proxies for us, but other providers might not.
				if (methodName == "GetObjectData")
				{
					SerializationInfo info = (SerializationInfo)args[0];
					StreamingContext context = (StreamingContext)args[1]; // not used !?!
					if (Target == null & Session != null)
					{
						EntityKey key = Session.GenerateEntityKey(Identifier, Session.Factory.GetEntityPersister(EntityName));
						object entity = Session.PersistenceContext.GetEntity(key);
						if (entity != null)
							SetImplementation(entity);
					}

					// let the specific ILazyInitializer write its requirements for deserialization 
					// into the stream.
					AddSerializationInfo(info, context);
					// don't need a return value for proxy.
					return null;
				}
			}

			//if it is a property of an embedded component, invoke on the "identifier"
			if (componentIdType != null && componentIdType.IsMethodOf(method))
			{
				return method.Invoke(Identifier, args);
			}

			return InvokeImplementation;
		}
	}
}