using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2923
{
	public partial class Parent
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ICollection<Child> Children { get; set; }
	}

	public partial class Child
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual Parent Parent { get; set; }
	}
}