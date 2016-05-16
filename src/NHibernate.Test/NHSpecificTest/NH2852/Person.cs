namespace NHibernate.Test.NHSpecificTest.NH2852
{
	public partial class Person
	{
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual Person Parent { get; set; }

		public virtual Address Address { get; set; }
	}

	public partial class Address
	{
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual City City { get; set; }
	}

	public partial class City
	{
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }
	}
}