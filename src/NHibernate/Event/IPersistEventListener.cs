using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary>
	/// Defines the contract for handling of create events generated from a session.
	/// </summary>
	public interface IPersistEventListener
	{
		/// <summary> Handle the given create event.</summary>
		/// <param name="event">The create event to be handled.</param>
		/// <param name="async"></param>
		Task OnPersist(PersistEvent @event, bool async);

		/// <summary> Handle the given create event. </summary>
		/// <param name="event">The create event to be handled.</param>
		/// <param name="createdAlready"></param>
		/// <param name="async"></param>
		Task OnPersist(PersistEvent @event, IDictionary createdAlready, bool async);
	}
}