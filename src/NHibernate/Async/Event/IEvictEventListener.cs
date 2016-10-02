#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IEvictEventListener
	{
		/// <summary> Handle the given evict event. </summary>
		/// <param name = "event">The evict event to be handled.</param>
		Task OnEvictAsync(EvictEvent @event);
	}
}
#endif
