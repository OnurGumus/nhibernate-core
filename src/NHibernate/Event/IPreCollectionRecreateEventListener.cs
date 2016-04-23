using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary> Called before recreating a collection </summary>
	public interface IPreCollectionRecreateEventListener
	{
		Task OnPreRecreateCollection(PreCollectionRecreateEvent @event);
	}
}