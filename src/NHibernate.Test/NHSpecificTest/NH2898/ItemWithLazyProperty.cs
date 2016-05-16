namespace NHibernate.Test.NHSpecificTest.NH2898
{
	public partial class ItemWithLazyProperty
	{
		public virtual int Id { get; set; }

		public virtual string Name { get; set; }

		public virtual string Description { get; set; }
	}
}