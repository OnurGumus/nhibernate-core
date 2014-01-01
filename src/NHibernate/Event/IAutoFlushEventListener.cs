using System.Threading.Tasks;
namespace NHibernate.Event
{
	/// <summary> Defines the contract for handling of session auto-flush events. </summary>
	public interface IAutoFlushEventListener
	{
		/// <summary>
		/// Handle the given auto-flush event.
		/// </summary>
		/// <param name="event">The auto-flush event to be handled.</param>
		/// <param name="async"></param>
		Task OnAutoFlush(AutoFlushEvent @event, bool async);
	}
}