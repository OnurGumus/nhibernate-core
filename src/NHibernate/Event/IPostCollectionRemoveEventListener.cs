using System.Threading.Tasks;

namespace NHibernate.Event
{
	/// <summary> Called after removing a collection </summary>
	public interface IPostCollectionRemoveEventListener
	{
		Task OnPostRemoveCollection(PostCollectionRemoveEvent @event);
	}
}