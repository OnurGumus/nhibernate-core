namespace NHibernate.Test.Unionsubclass2
{
	public partial class Employee:Person
	{
		private string title;
		private decimal salary;
		private Employee manager;

		public virtual string Title
		{
			get { return title; }
			set { title = value; }
		}

		public virtual decimal Salary
		{
			get { return salary; }
			set { salary = value; }
		}

		public virtual Employee Manager
		{
			get { return manager; }
			set { manager = value; }
		}
	}
}
