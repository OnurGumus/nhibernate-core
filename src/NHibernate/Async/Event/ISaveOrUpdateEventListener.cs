#if NET_4_5
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ISaveOrUpdateEventListener
	{
		/// <summary> Handle the given update event. </summary>
		/// <param name = "event">The update event to be handled.</param>
		Task OnSaveOrUpdateAsync(SaveOrUpdateEvent @event);
	}
}
#endif
