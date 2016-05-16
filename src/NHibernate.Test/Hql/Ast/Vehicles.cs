namespace NHibernate.Test.Hql.Ast
{
	public partial class Vehicle
	{
		private long id;
		private string vin;
		private string owner;

		public virtual long Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Vin
		{
			get { return vin; }
			set { vin = value; }
		}

		public virtual string Owner
		{
			get { return owner; }
			set { owner = value; }
		}
	}

	public partial class Car : Vehicle
	{
	}

	public partial class Truck : Vehicle
	{
	}

	public partial class Pickup : Truck
	{
	}

	public partial class SUV : Truck
	{
	}
}