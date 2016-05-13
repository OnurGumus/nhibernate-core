using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary> Defines the contract for handling of evict events generated from a session. </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IEvictEventListener
	{
		/// <summary> Handle the given evict event. </summary>
		/// <param name = "event">The evict event to be handled.</param>
		Task OnEvictAsync(EvictEvent @event);
	}
}