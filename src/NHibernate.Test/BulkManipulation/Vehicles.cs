namespace NHibernate.Test.BulkManipulation
{
	public partial class Vehicle
	{
		private long id;
		private string vin;
		private string owner;

		public long Id
		{
			get { return id; }
			set { id = value; }
		}

		public string Vin
		{
			get { return vin; }
			set { vin = value; }
		}

		public string Owner
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