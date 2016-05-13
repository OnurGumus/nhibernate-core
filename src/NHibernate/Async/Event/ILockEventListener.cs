using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of lock events generated from a session.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ILockEventListener
	{
		/// <summary>Handle the given lock event. </summary>
		/// <param name = "event">The lock event to be handled. </param>
		Task OnLockAsync(LockEvent @event);
	}
}