#if NET_4_5
using System.Collections;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of merge events generated from a session.
	/// </summary>
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
