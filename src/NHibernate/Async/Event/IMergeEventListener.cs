#if NET_4_5
using System.Collections;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Event
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IMergeEventListener
	{
		/// <summary> Handle the given merge event. </summary>
		/// <param name = "event">The merge event to be handled. </param>
		Task OnMergeAsync(MergeEvent @event);
		/// <summary> Handle the given merge event. </summary>
		/// <param name = "event">The merge event to be handled. </param>
		/// <param name = "copiedAlready"></param>
		Task OnMergeAsync(MergeEvent @event, IDictionary copiedAlready);
	}
}
#endif
