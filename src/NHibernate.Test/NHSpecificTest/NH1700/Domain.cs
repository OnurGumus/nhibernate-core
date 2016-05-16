namespace NHibernate.Test.NHSpecificTest.NH1700
{
	public partial class PayrollSegment
	{
		public virtual string Id { get; set; }
	}
	public partial class ActualPayrollSegment : PayrollSegment
	{
	}
	public partial class ProjectedPayrollSegment : PayrollSegment
	{
	}
	public partial class ClosedPayrollSegment : PayrollSegment
	{
	}
}