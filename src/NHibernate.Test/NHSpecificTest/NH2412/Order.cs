using System;
using System.Collections;

namespace NHibernate.Test.NHSpecificTest.NH2412
{
	public partial class Order
	{
		public int? Id { get; set; }
		public Customer Customer { get; set; }
	}
}
