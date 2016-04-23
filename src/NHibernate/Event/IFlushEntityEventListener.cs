using System.Threading.Tasks;

namespace NHibernate.Event
{
	public interface IFlushEntityEventListener
	{
		Task OnFlushEntity(FlushEntityEvent @event);
	}
}