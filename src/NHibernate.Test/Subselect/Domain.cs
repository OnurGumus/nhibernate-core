namespace NHibernate.Test.Subselect
{
	public partial class Human
	{
		public virtual int Id { get; set; }
		public virtual char Sex { get; set; }
		public virtual string Name { get; set; }
		public virtual string Address { get; set; }
	}

	public partial class Alien
	{
		public virtual int Id { get; set; }
		public virtual string Identity { get; set; }
		public virtual string Planet { get; set; }
		public virtual string Species { get; set; }
	}

	public partial class Being
	{
		public virtual int Id { get; set; }
		public virtual string Identity { get; set; }
		public virtual string Location { get; set; }
		public virtual string Species { get; set; }
	}
}