using System.Data;
using NHibernate.SqlCommand;
using System.Threading.Tasks;

namespace NHibernate.Driver
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IResultSetsCommand
	{
		Task<IDataReader> GetReaderAsync(int ? commandTimeout);
	}
}