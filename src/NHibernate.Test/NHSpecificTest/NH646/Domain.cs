using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH646
{
	public partial class Person
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class Station
	{
		public virtual Guid Id { get; set; }

		public virtual ICollection<Policeman> Policemen { get; set; }
	}

	public partial class Policeman : Person
	{
		public virtual Station Station { get; set; }
	}
}
