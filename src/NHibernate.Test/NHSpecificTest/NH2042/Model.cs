using System;

namespace NHibernate.Test.NHSpecificTest.NH2042
{
	public partial class Person
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class Owner : Person
	{
		public virtual string SomeProperty { get; set; }
		public virtual long Test { get; set; }
	}
}
