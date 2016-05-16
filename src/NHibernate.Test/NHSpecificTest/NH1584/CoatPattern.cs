namespace NHibernate.Test.NHSpecificTest.NH1584
{
	public abstract partial class CoatPattern
	{
		public virtual int Id { get; private set; }

		public virtual string Description { get; set; }
	}
}