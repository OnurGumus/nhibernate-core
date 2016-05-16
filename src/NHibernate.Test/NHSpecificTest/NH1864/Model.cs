using System;

namespace NHibernate.Test.NHSpecificTest.NH1864
{
	public partial class Category
	{
		public virtual int ID { get; protected set; }
		public virtual DateTime ValidUntil { get; set; }
	}

	public partial class Invoice
	{
		public virtual int ID { get; protected set; }
		public virtual DateTime ValidUntil { get; set; }
		public virtual int Foo { get; set; }
	}
}
