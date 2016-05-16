using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH1391
{
	public partial class Cat:Animal
	{
		public virtual string EyeColor { get; set; }
	}
}
