namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Fk.Bidirectional
{
	public partial class Employee
	{
		public virtual long Id { get; set; }
		public virtual string Name { get; set; }
		public virtual EmployeeInfo Info { get; set; }
	}

	public partial class EmployeeInfo
	{
		public virtual long Id { get; set; }
		public virtual Employee EmployeeDetails { get; set; }

		public EmployeeInfo()
		{
		}

		public EmployeeInfo(Employee emp)
		{
			EmployeeDetails = emp;
		}
	}
}
