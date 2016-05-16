namespace NHibernate.Test.NHSpecificTest.NH2328
{
	public partial class ToyBox
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IShape Shape { get; set; }
	}

	public interface IShape
	{
	}

	public partial class Circle : IShape
	{
		public virtual int Id { get; set; }
	}

	public partial class Square : IShape
	{
		public virtual int Id { get; set; }
	}
}