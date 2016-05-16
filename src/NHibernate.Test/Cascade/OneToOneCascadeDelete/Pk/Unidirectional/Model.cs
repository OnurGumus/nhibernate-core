namespace NHibernate.Test.Cascade.OneToOneCascadeDelete.Pk.Unidirectional
{
	public partial class Employee
	{
		public virtual long Id { get; set; }
		public virtual string Name { get; set; }
		public virtual EmployeeInfo Info { get; set; }

		public Employee()
		{

		}
	}

	public partial class EmployeeInfo
	{
		public virtual long Id { get; set; }

		public EmployeeInfo()
		{

		}

		public EmployeeInfo(long id)
		{
			this.Id = id;
		}
	}
}
