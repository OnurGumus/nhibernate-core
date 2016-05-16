using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2228
{
	public partial class Parent
	{
		public Parent()
		{
			Children = new List<Child>();
		}
		public virtual int Id { get; set; }
		public virtual IList<Child> Children { get; set; }
	}
	public partial class Child
	{
		public virtual int Id { get; set; }
		public virtual Parent Parent { get; set; }
		public virtual string Description { get; set; }
	}
}