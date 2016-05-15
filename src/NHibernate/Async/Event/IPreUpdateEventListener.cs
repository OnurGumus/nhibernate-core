#if NET_4_5
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary>
	/// Called before updating the datastore
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPreUpdateEventListener
	{
		/// <summary> Return true if the operation should be vetoed</summary>
		/// <param name = "event"></param>
		Task<bool> OnPreUpdateAsync(PreUpdateEvent @event);
	}
}
#endif
