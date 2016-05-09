using System.Data;

namespace NHibernate.Dialect
{
	public partial class MySQL55Dialect : MySQL5Dialect
	{
		public MySQL55Dialect()
		{
			RegisterColumnType(DbType.Guid, "CHAR(36)");
		}
	}
}