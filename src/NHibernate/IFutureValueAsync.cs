#if NET_4_5
using System.Threading.Tasks;

namespace NHibernate
{
	public interface IFutureValueAsync<T>
	{
		Task<T> GetValue();
	}
}
#endif