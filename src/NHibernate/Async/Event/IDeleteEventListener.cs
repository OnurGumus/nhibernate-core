#if NET_4_5
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary> Defines the contract for handling of deletion events generated from a session. </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IDeleteEventListener
	{
		/// <summary>Handle the given delete event. </summary>
		/// <param name = "event">The delete event to be handled. </param>
		Task OnDeleteAsync(DeleteEvent @event);
		Task OnDeleteAsync(DeleteEvent @event, ISet<object> transientEntities);
	}
}
#endif
