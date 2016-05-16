using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH1727
{
	public partial class ClassA
	{
		public virtual Guid Id { get; set; }
		public virtual IList<ClassB> BCollection { get; set; }
		public virtual string Name { get; set; }
		public virtual int Value { get; set; }
		public virtual ClassB B {get;set;}
	}

	public partial class ClassB
	{
		public virtual Guid Id { get; set; }
	}
}