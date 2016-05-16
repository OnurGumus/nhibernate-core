using System;
using System.Collections;
using System.Collections.Generic;

namespace NHibernate.Test.MappingTest
{
	public partial class Wicked
	{
		public int Id { get; set; }
		public int VersionProp { get; set; }
		public MonetaryAmount Component { get; set; }
		public ISet<Employee> SortedEmployee { get; set; }
		public IList AnotherSet { get; set; }
	}

	public partial class MonetaryAmount
	{
		public string X { get; set; }
	}

	public partial class Employee
	{
		public int Id { get; set; }
		public string Emp { get; set; }
		public Employee Empinone { get; set; }
	}

	public partial class Animal
	{
		public virtual string Description { get; set; }
	}

	public partial class Reptile : Animal
	{
		public virtual double BodyTemperature { get; set; }
	}

	public partial class Lizard : Reptile { }

	public partial class Mammal : Animal
	{
		public virtual bool Pregnant { get; set; }
		public virtual DateTime Birthdate { get; set; }
	}

	public partial class DomesticAnimal : Animal
	{
		public virtual string Name { get; set; }
		public virtual Employee Owner { get; set; }
	}

	public partial class Cat : Animal { }

	public partial class Dog : Animal { }
}