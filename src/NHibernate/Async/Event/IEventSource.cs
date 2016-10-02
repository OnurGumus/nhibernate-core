#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IEventSource : ISessionImplementor, ISession
	{
		/// <summary> Force an immediate flush</summary>
		Task ForceFlushAsync(EntityEntry e);
		/// <summary> Cascade merge an entity instance</summary>
		Task MergeAsync(string entityName, object obj, IDictionary copiedAlready);
		/// <summary> Cascade persist an entity instance</summary>
		Task PersistAsync(string entityName, object obj, IDictionary createdAlready);
		/// <summary> Cascade persist an entity instance during the flush process</summary>
		Task PersistOnFlushAsync(string entityName, object obj, IDictionary copiedAlready);
		/// <summary> Cascade refresh an entity instance</summary>
		Task RefreshAsync(object obj, IDictionary refreshedAlready);
		/// <summary> Cascade delete an entity instance</summary>
		Task DeleteAsync(string entityName, object child, bool isCascadeDeleteEnabled, ISet<object> transientEntities);
	}
}
#endif
