using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH1579
{
	public partial class Orange : Fruit
	{
		public Orange(Entity container)
			: base(container)
		{
		}

		protected Orange()
		{
		}
	}
}
