using System.Data;
using System.Threading.Tasks;

namespace NHibernate.Driver
{
	public interface IDataReaderEx : IDataReader
	{
		Task<bool> ReadAsync();

		Task<bool> NextResultAsync();
	}
}
