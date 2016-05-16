namespace NHibernate.Test.NHSpecificTest.NH2041
{
	public partial class MyClass
	{
		public Coordinates Location { get; set; }
	}

	public partial class Coordinates
	{
		public decimal Latitude { get; set; }
		public decimal Longitude { get; set; }
	}
}