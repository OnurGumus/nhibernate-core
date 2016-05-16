
namespace NHibernate.Test.NHSpecificTest.NH3145
{
	public partial class Root
	{
		public virtual int Id { get; set; }
		public virtual Base Base { get; set; }
	}

	public partial class Base
	{
		public virtual int Id { get; set; }
		public virtual string LongContent { get; set; }
	}

	public partial class Derived : Base
	{
	}
}
