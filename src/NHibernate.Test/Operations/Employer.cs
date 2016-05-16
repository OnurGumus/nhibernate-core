using System.Collections.Generic;

namespace NHibernate.Test.Operations
{
	public partial class Employer
	{
		public virtual int Id { get; set; }
		public virtual ICollection<Employee> Employees { get; set; }
		public virtual int Vers { get; set; }
	}
}