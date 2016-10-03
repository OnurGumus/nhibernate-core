#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IFlushEventListener
	{
		/// <summary>Handle the given flush event. </summary>
		/// <param name = "event">The flush event to be handled.</param>
		Task OnFlushAsync(FlushEvent @event);
	}
}
#endif
