using System;

namespace NHibernate.Test.NHSpecificTest.NH3139
{
	public partial class Product
	{
		public virtual Guid Id { get; set; }

		public virtual string Name { get; set; }

		public virtual Inventory Inventory { get; set; }

		public virtual Brand Brand { get; set; }
	}

	public partial class Inventory
	{
		public virtual Guid Id { get; set; }

		public virtual int Quantity { get; set; }
	}

	public partial class Brand
	{
		public virtual Guid Id { get; set; }

		public virtual string Name { get; set; }
	}
}
