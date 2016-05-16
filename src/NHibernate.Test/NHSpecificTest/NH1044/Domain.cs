using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH1044
{
	public partial class Person
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual Delivery Delivery { get; set; }
	}

	public partial class Delivery
	{
		public Delivery()
		{
			Adresses = new List<string>();
		}
		public virtual IList<string> Adresses { get; set; }
	}
}