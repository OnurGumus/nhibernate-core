using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary> Called before removing a collection </summary>
	public interface IPreCollectionRemoveEventListener
	{
		Task OnPreRemoveCollection(PreCollectionRemoveEvent @event);
	}
}