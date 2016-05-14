#if NET_4_5
using System.Collections;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of refresh events generated from a session.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IRefreshEventListener
	{
		/// <summary> Handle the given refresh event. </summary>
		/// <param name = "event">The refresh event to be handled.</param>
		Task OnRefreshAsync(RefreshEvent @event);
		/// <summary>
		/// 
		/// </summary>
		/// <param name = "event"></param>
		/// <param name = "refreshedAlready"></param>
		Task OnRefreshAsync(RefreshEvent @event, IDictionary refreshedAlready);
	}
}
#endif
