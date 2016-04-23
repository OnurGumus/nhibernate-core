using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary> Called before updating a collection </summary>
	public interface IPreCollectionUpdateEventListener
	{
		Task OnPreUpdateCollection(PreCollectionUpdateEvent @event);
	}
}