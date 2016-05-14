#if NET_4_5
using System.Collections;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of create events generated from a session.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPersistEventListener
	{
		/// <summary> Handle the given create event.</summary>
		/// <param name = "event">The create event to be handled.</param>
		Task OnPersistAsync(PersistEvent @event);
		/// <summary> Handle the given create event. </summary>
		/// <param name = "event">The create event to be handled.</param>
		/// <param name = "createdAlready"></param>
		Task OnPersistAsync(PersistEvent @event, IDictionary createdAlready);
	}
}
#endif
