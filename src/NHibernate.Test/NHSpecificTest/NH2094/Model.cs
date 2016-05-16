using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2094
{
	public partial class Person
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		public virtual string LazyField { get; set; }
	}
}
