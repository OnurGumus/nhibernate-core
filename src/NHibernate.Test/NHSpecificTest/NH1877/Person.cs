using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH1877
{
	public partial class Person
	{
		public virtual long Id { get; set; }
		public virtual DateTime BirthDate { get; set; }
	}
}
