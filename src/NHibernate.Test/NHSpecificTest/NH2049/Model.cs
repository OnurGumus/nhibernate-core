

namespace NHibernate.Test.NHSpecificTest.NH2049
{
	public abstract partial class Customer
	{
		public int Id { get; set; }
		public bool Deleted { get; set; }
	}

	public partial class IndividualCustomer : Customer
	{
		public Person Person { get; set; }
	}

	public partial class Person
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public IndividualCustomer IndividualCustomer { get; set; }
	}
}