using System;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Cache.Entry
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public sealed partial class CacheEntry
	{
		public async Task<object[]> AssembleAsync(object instance, object id, IEntityPersister persister, IInterceptor interceptor, ISessionImplementor session)
		{
			if (!persister.EntityName.Equals(subclass))
			{
				throw new AssertionFailure("Tried to assemble a different subclass instance");
			}

			return await (AssembleAsync(disassembledState, instance, id, persister, interceptor, session));
		}

		private static async Task<object[]> AssembleAsync(object[] values, object result, object id, IEntityPersister persister, IInterceptor interceptor, ISessionImplementor session)
		{
			//assembled state gets put in a new array (we read from cache by value!)
			object[] assembledProps = await (TypeHelper.AssembleAsync(values, persister.PropertyTypes, session, result));
			//from h3.2 TODO: reuse the PreLoadEvent
			PreLoadEvent preLoadEvent = new PreLoadEvent((IEventSource)session);
			preLoadEvent.Entity = result;
			preLoadEvent.State = assembledProps;
			preLoadEvent.Id = id;
			preLoadEvent.Persister = persister;
			IPreLoadEventListener[] listeners = session.Listeners.PreLoadEventListeners;
			for (int i = 0; i < listeners.Length; i++)
			{
				listeners[i].OnPreLoad(preLoadEvent);
			}

			persister.SetPropertyValues(result, assembledProps, session.EntityMode);
			return assembledProps;
		}
	}
}