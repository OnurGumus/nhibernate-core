#if NET_4_5
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of update events generated from a session.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ISaveOrUpdateEventListener
	{
		/// <summary> Handle the given update event. </summary>
		/// <param name = "event">The update event to be handled.</param>
		Task OnSaveOrUpdateAsync(SaveOrUpdateEvent @event);
	}
}
#endif
