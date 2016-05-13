using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary> Defines the contract for handling of session auto-flush events. </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IAutoFlushEventListener
	{
		/// <summary>
		/// Handle the given auto-flush event.
		/// </summary>
		/// <param name = "event">The auto-flush event to be handled.</param>
		Task OnAutoFlushAsync(AutoFlushEvent @event);
	}
}