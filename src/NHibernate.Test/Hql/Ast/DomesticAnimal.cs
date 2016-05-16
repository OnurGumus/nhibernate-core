namespace NHibernate.Test.Hql.Ast
{
	public partial class DomesticAnimal: Mammal
	{
		private Human owner;

		public virtual Human Owner
		{
			get { return owner; }
			set { owner = value; }
		}
	}

	public partial class Cat : DomesticAnimal { }
	public partial class Dog : DomesticAnimal { }
}