using System.Data;

namespace NHibernate.Driver
{
	public partial interface IDriveConnectionCommandProvider
	{
		IDbConnection CreateConnection();
		IDbCommand CreateCommand();
	}
}