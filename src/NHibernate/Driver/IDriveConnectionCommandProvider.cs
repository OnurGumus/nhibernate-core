using System.Data.Common;

namespace NHibernate.Driver
{
	public partial interface IDriveConnectionCommandProvider
	{
		DbConnection CreateConnection();
		DbCommand CreateCommand();
	}
}