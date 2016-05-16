using System;

namespace NHibernate.Test.NHSpecificTest.NH1549
{
	public partial class CategoryWithInheritedId : EntityInt32
	{
		public virtual string Name { get; set; }
	}

	public partial class CategoryWithId
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}
}