using System;

namespace NHibernate.Test.NHSpecificTest.NH1908
{
	public partial class Category
	{
		public virtual int ID { get; protected set; }
		public virtual Category ParentCategory { get; set; }
		public virtual DateTime ValidUntil { get; set; }
	}

	public partial class Invoice
	{
		public virtual int ID { get; protected set; }
		public virtual DateTime Issued { get; set; }
		public virtual Category Category { get; set; }
	}
}
