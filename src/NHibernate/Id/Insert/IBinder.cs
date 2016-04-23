using System.Data;
using System.Threading.Tasks;

namespace NHibernate.Id.Insert
{
	public interface IBinder
	{
		object Entity { get;}
		Task BindValues(IDbCommand cm);
	}
}