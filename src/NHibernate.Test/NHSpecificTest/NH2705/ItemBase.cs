namespace NHibernate.Test.NHSpecificTest.NH2705
{
	public partial class ItemBase
	{
		public virtual int Id { get; set; }
		public virtual SubItemBase SubItem { get; set; }
	}

	public partial class ItemWithComponentSubItem : ItemBase {}
}