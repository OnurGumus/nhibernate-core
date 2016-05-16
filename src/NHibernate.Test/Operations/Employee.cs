using System.Collections.Generic;

namespace NHibernate.Test.Operations
{
	public partial class Employee
	{
		public virtual int Id { get; set; }
		public virtual ICollection<Employer> Employers { get; set; }
	}
}