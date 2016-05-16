namespace NHibernate.Test.PolymorphicGetAndLoad
{
	public partial class A: INamed
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}

	public interface IOccuped
	{
		string Occupation { get; set; }
	}

	public partial class B: A, IOccuped
	{
		public virtual string Occupation { get; set; }
	}

	public interface INamed
	{
		string Name	{ get; set; }
	}

	public partial class GraphA : IMultiGraphNamed
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}

	public partial class GraphB : IMultiGraphNamed
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
	}

	public interface IMultiGraphNamed
	{
		string Name { get; set; }
	}

}