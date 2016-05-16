namespace NHibernate.Test.NHSpecificTest.NH1635
{
	public partial class ForumMessage
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual ForumThread ForumThread { get; set; }
	}
}