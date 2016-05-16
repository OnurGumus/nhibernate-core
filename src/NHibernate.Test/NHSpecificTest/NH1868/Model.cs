using System;

namespace NHibernate.Test.NHSpecificTest.NH1868
{
	public partial class Category
	{
		public virtual int ID { get; protected set; }
		public virtual DateTime ValidUntil { get; set; }
	}

	public partial class Package
	{
		public virtual int ID { get; protected set; }
		public virtual DateTime ValidUntil { get; set; }
	}

	public partial class Invoice
	{
		public virtual int ID { get; protected set; }
		public virtual DateTime ValidUntil { get; set; }
		public virtual Category Category { get; set; }
		public virtual Package Package { get; set; }
	}
}
