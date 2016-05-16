namespace NHibernate.Test.NHSpecificTest.NH1584
{
	public abstract partial class Cat
	{
		public virtual int Id { get; protected set; }

		public virtual string Name { get; set; }
	}
}