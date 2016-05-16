using System;
using System.Collections;
using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH2721
{
	public partial class B
	{
		public B()
		{
		}

		public B(string name)
		{
			Name = name;
		}

	    public int? Id { get; set; }
	    public string Name { get; set; }
        public A A { get; set; }
	}
}
