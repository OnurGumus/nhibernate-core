namespace NHibernate.Test.Events.Collections.Association.Bidirectional.OneToMany
{
	public partial class ParentWithBidirectionalOneToManySubclass : ParentWithBidirectionalOneToMany
	{
		public ParentWithBidirectionalOneToManySubclass() {}
		public ParentWithBidirectionalOneToManySubclass(string name) : base(name) {}
	}
}