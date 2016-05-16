using System;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH1812
{
	public partial class Person
	{
		public virtual int Id {get; set;}
		public virtual IList<Period> PeriodCollection { get; set; }

		public Person(){PeriodCollection=new List<Period>();}
	}

	public partial class Period
	{
		public virtual int Id { get; set; }
		public virtual DateTime? Start { get; set; }
	}
}