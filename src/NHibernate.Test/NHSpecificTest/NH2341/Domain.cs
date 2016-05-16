namespace NHibernate.Test.NHSpecificTest.NH2341
{
	public abstract partial class AbstractBA
	{
		public virtual int Id { get; set; }
	}
	public partial class ConcreteBA : AbstractBA
	{
	}
	public partial class ConcreteA : ConcreteBA
	{
	}
	public partial class ConcreteB : ConcreteBA
	{
	}
}