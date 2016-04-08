using System.Data;
using System.Threading.Tasks;
using NHibernate.SqlCommand;

namespace NHibernate.Driver
{
	public interface IResultSetsCommand
	{
		void Append(ISqlCommand command);
		bool HasQueries { get; }
		SqlString Sql { get; }
		Task<IDataReader> GetReader(int? commandTimeout, bool async);
	}
}