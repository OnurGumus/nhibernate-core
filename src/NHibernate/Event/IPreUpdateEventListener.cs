using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary>
	/// Called before updating the datastore
	/// </summary>
	public interface IPreUpdateEventListener
	{
		/// <summary> Return true if the operation should be vetoed</summary>
		/// <param name="event"></param>
		Task<bool> OnPreUpdate(PreUpdateEvent @event);
	}
}