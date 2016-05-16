using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH3489
{
	public partial class Order
	{
		public Order()
		{
			Departments = new HashSet<Department>();
		}

		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		public virtual ISet<Department> Departments { get; set; }
	}
}