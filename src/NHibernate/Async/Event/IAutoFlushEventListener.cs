#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
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
#endif
