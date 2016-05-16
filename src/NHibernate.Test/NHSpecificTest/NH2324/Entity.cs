namespace NHibernate.Test.NHSpecificTest.NH2324
{
	public partial class Entity
	{
		public virtual long Id { get; set; }

		public virtual CompositeData Data { get; set; }
	}
}