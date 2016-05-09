using System;
using NHibernate.Type;

namespace NHibernate.Criterion
{
	[Serializable]
	public partial class RowCountInt64Projection : RowCountProjection
	{
		public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
		{
			return new IType[] { NHibernateUtil.Int64 };
		}
	}
}