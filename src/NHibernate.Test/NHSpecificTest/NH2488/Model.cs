namespace NHibernate.Test.NHSpecificTest.NH2488
{
	public partial class Base1
	{
		public virtual int Id { get; set; }

		public virtual string ShortContent { get; set; }
	}

	public partial class Base2
	{
		public virtual int Id { get; set; }

		public virtual string ShortContent { get; set; }
	}

	public partial class Derived1 : Base1
	{
		public virtual string LongContent { get; set; }
	}

	public partial class Derived2 : Base2
	{
		public virtual string LongContent { get; set; }
	}

	public partial class Base3
	{
		public virtual int Id { get; set; }

		public virtual string ShortContent { get; set; }
	}
	public partial class Derived3 : Base3
	{
		public virtual string LongContent { get; set; }
	}
}