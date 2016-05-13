using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary> Defines the contract for handling of session flush events. </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IFlushEventListener
	{
		/// <summary>Handle the given flush event. </summary>
		/// <param name = "event">The flush event to be handled.</param>
		Task OnFlushAsync(FlushEvent @event);
	}
}