using System.Data;
using NHibernate.SqlCommand;

namespace NHibernate.Driver
{
	public partial interface IResultSetsCommand
	{
		void Append(ISqlCommand command);
		bool HasQueries { get; }
		SqlString Sql { get; }
		IDataReader GetReader(int? commandTimeout);
	}
}