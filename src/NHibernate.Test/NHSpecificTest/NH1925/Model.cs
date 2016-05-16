using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH1925
{
	public partial class Customer
	{
		public virtual int ID { get; protected set; }
		public virtual ISet<Invoice> Invoices { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class Invoice
	{
		public virtual int ID { get; protected set; }
		public virtual Customer Customer { get; set; }
		public virtual int Number { get; set; }
	}
}