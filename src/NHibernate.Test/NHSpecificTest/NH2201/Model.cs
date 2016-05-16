using System;

namespace NHibernate.Test.NHSpecificTest.NH2201
{
	public partial class Parent
	{
		public virtual Guid Id { get; set; }
	}

	public partial class SubClass : Parent
	{
		public virtual string Name { get; set; }
	}
}
