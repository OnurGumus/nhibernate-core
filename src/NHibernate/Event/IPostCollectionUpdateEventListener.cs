using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary> Called after updating a collection </summary>
	public interface IPostCollectionUpdateEventListener
	{
		Task OnPostUpdateCollection(PostCollectionUpdateEvent @event);
	}
}