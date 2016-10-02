#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IReplicateEventListener
	{
		/// <summary>Handle the given replicate event. </summary>
		/// <param name = "event">The replicate event to be handled.</param>
		Task OnReplicateAsync(ReplicateEvent @event);
	}
}
#endif
