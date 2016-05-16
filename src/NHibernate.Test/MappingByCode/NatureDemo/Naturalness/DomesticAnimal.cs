namespace NHibernate.Test.MappingByCode.NatureDemo.Naturalness
{
	public partial class DomesticAnimal: Mammal
	{
		public virtual Human Owner { get; set; }
	}

	public partial class Cat : DomesticAnimal { }
	public partial class Dog : DomesticAnimal { }
}