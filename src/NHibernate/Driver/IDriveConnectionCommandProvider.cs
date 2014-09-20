using System.Data;
using System.Data.Common;

namespace NHibernate.Driver
{
	public interface IDriveConnectionCommandProvider
	{
		IDbConnection CreateConnection();
		DbCommand CreateCommand();
	}
}