namespace NHibernate.Test.NHSpecificTest.NH1640
{
	public partial class Entity
	{
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual Entity Child { get; set; }
	}
}