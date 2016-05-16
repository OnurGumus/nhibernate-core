using System;
using System.Collections;

namespace NHibernate.Test.NHSpecificTest.NH2244
{
	public partial class A
	{
		public int? Id { get; set; }
		public PhoneNumber Phone { get; set; }
	}
}
