using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.CriteriaQueryOnComponentCollection
{
	public partial class Employee
	{
		public virtual int Id { get; set; }
		public virtual ICollection<Money> Amounts { get; set; }
		public virtual ICollection<ManagedEmployee> ManagedEmployees { get; set; }
	}

	public partial class ManagedEmployee
	{
		public virtual Employee Employee { get; set; }
		public virtual string Position { get; set; }
	}
}
