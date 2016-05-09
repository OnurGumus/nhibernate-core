using System.Data.Common;

namespace NHibernate.Exceptions
{
	public partial class NoOpViolatedConstraintNameExtracter : IViolatedConstraintNameExtracter
	{
		public virtual string ExtractConstraintName(DbException sqle)
		{
			return null;
		}
	}
}