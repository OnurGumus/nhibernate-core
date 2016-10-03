#if NET_4_5
using System.Collections;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IRefreshEventListener
	{
		/// <summary> Handle the given refresh event. </summary>
		/// <param name = "event">The refresh event to be handled.</param>
		Task OnRefreshAsync(RefreshEvent @event);
		/// <summary>
		/// 
		/// </summary>
		/// <param name = "event"></param>
		/// <param name = "refreshedAlready"></param>
		Task OnRefreshAsync(RefreshEvent @event, IDictionary refreshedAlready);
	}
}
#endif
