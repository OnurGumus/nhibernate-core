using System;

namespace NHibernate.Test.NHSpecificTest.NH2691
{
	public abstract partial class Animal
	{
		public virtual int Id { get; set; }
		public virtual string Description { get; set; }
		public virtual int Sequence { get; set; }
	}

	public abstract partial class Reptile : Animal
	{
		public virtual double BodyTemperature { get; set; }
	}

	public partial class Lizard : Reptile { }

	public abstract partial class Mammal : Animal
	{
		public virtual bool Pregnant { get; set; }
		public virtual DateTime? BirthDate { get; set; }
	}

	public partial class Dog : Mammal { }

	public partial class Cat : Mammal { }
}