using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary> Called after recreating a collection </summary>
	public interface IPostCollectionRecreateEventListener
	{
		Task OnPostRecreateCollection(PostCollectionRecreateEvent @event);
	}
}