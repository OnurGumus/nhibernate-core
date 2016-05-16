namespace NHibernate.Test.NHSpecificTest.NH2408
{
	public partial class Animal
	{
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }
	}

	public partial class Dog : Animal
	{
	}

	public partial class Cat : Animal
	{
	}
}
