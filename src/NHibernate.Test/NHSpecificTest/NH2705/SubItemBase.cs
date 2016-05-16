namespace NHibernate.Test.NHSpecificTest.NH2705
{
	// NOTE: an Entity and a Component in the same hierarchy is not supported
	// we are using this trick just to ""simplify"" the test.
	public partial class SubItemBase
	{
		public virtual string Name { get; set; }
		public virtual SubItemDetails Details { get; set; }
	}

	public partial class SubItemComponent : SubItemBase {}
}