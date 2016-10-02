#if NET_4_5
using System.Collections;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IPersistEventListener
	{
		/// <summary> Handle the given create event.</summary>
		/// <param name = "event">The create event to be handled.</param>
		Task OnPersistAsync(PersistEvent @event);
		/// <summary> Handle the given create event. </summary>
		/// <param name = "event">The create event to be handled.</param>
		/// <param name = "createdAlready"></param>
		Task OnPersistAsync(PersistEvent @event, IDictionary createdAlready);
	}
}
#endif
