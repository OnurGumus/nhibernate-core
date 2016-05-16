using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH1579
{
	public partial class Apple : Fruit
	{
		public Apple(Entity container)
			: base(container)
		{
		}

		protected Apple()
		{
		}
	}
}
