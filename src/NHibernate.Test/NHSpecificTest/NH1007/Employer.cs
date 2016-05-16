using System;

namespace NHibernate.Test.NHSpecificTest.NH1007
{
	public partial class Employer1
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class Employer2
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
	}
}