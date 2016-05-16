namespace NHibernate.Test.Events.Collections.Association.Bidirectional.OneToMany
{
	public partial class ChildWithManyToOne : ChildEntity
	{
		private IParentWithCollection parent;

		public ChildWithManyToOne() {}
		public ChildWithManyToOne(string name) : base(name) {}

		public virtual IParentWithCollection Parent
		{
			get { return parent; }
			set { parent = value; }
		}
	}
}