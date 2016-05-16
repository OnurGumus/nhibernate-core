using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH1391
{
	public partial class Dog:Animal
	{
		public virtual string Country { get; set; }
	}
}
