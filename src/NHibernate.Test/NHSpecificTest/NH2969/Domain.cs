namespace NHibernate.Test.NHSpecificTest.NH2969
{
	public partial class Person
	{
		public virtual int ID { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class Cat
	{
		public virtual int ID { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class DomesticCat : Cat
	{
		public virtual Person Owner { get; set; }
	}

	public partial class Fish
	{
		public virtual int ID { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class Goldfish : Fish
	{
		public virtual Person Owner { get; set; }
	}

	public partial class Bird
	{
		public virtual int ID { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class Parrot : Bird
	{
		public virtual Person Pirate { get; set; }
	}
}
