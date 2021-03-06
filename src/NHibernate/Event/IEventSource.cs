using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;

namespace NHibernate.Event
{
	public interface IEventSource : ISessionImplementor, ISession
	{
		/// <summary> Get the ActionQueue for this session</summary>
		ActionQueue ActionQueue { get;}

		/// <summary> 
		/// Instantiate an entity instance, using either an interceptor,
		/// or the given persister
		/// </summary>
		object Instantiate(IEntityPersister persister, object id);

		/// <summary> Force an immediate flush</summary>
		Task ForceFlush(EntityEntry e);

		/// <summary> Cascade merge an entity instance</summary>
		Task Merge(string entityName, object obj, IDictionary copiedAlready);

		/// <summary> Cascade persist an entity instance</summary>
		Task Persist(string entityName, object obj, IDictionary createdAlready);

		/// <summary> Cascade persist an entity instance during the flush process</summary>
		Task PersistOnFlush(string entityName, object obj, IDictionary copiedAlready);

		/// <summary> Cascade refresh an entity instance</summary>
		Task Refresh(object obj, IDictionary refreshedAlready);

		Task Delete(string entityName, object child, bool isCascadeDeleteEnabled, ISet<object> transientEntities);
	}
}
