namespace NHibernate.Test.NHSpecificTest.NH1776
{
	public partial class Category
	{
		public virtual int Id { get; set; }
		public virtual string Code { get; set; }
		public virtual bool Deleted { get; set; }
	}
}