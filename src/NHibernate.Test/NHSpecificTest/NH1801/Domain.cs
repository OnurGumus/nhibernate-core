namespace NHibernate.Test.NHSpecificTest.NH1801
{
	public partial class A
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class B
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		public virtual A A { get; set; }
	}

	public partial class C
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }

		public virtual A A { get; set; }
	}
}