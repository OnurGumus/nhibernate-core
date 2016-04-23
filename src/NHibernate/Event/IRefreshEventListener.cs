using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of refresh events generated from a session.
	/// </summary>
	public interface IRefreshEventListener
	{
		/// <summary> Handle the given refresh event. </summary>
		/// <param name="event">The refresh event to be handled.</param>
		Task OnRefresh(RefreshEvent @event);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="event"></param>
		/// <param name="refreshedAlready"></param>
		Task OnRefresh(RefreshEvent @event, IDictionary refreshedAlready);
	}
}