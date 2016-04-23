using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of merge events generated from a session.
	/// </summary>
	public interface IMergeEventListener
	{
		/// <summary> Handle the given merge event. </summary>
		/// <param name="event">The merge event to be handled. </param>
		Task OnMerge(MergeEvent @event);

		/// <summary> Handle the given merge event. </summary>
		/// <param name="event">The merge event to be handled. </param>
		/// <param name="copiedAlready"></param>
		Task OnMerge(MergeEvent @event, IDictionary copiedAlready);
	}
}