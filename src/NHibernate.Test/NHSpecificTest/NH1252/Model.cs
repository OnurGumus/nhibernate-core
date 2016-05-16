namespace NHibernate.Test.NHSpecificTest.NH1252
{
	public partial class SomeClass
	{
		private int id;
		private string name;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}
	}

	public partial class SubClass1 : SomeClass {}
	public partial class SubClass2 : SomeClass { }
}
