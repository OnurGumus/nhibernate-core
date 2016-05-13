using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of replicate events generated from a session.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IReplicateEventListener
	{
		/// <summary>Handle the given replicate event. </summary>
		/// <param name = "event">The replicate event to be handled.</param>
		Task OnReplicateAsync(ReplicateEvent @event);
	}
}