namespace NHibernate.Test.NHSpecificTest.NH3074
{
	public partial class Animal
	{
		public virtual int Id { get; set; }
		public virtual int Weight { get; set; }
	}

	public partial class Cat : Animal
	{
		public virtual int NumberOfLegs { get; set; }
	}
}