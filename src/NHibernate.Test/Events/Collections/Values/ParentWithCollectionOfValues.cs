namespace NHibernate.Test.Events.Collections.Values
{
	public partial class ParentWithCollectionOfValues : AbstractParentWithCollection
	{
		public ParentWithCollectionOfValues() {}

		public ParentWithCollectionOfValues(string name) : base(name) {}

		public override IChild CreateChild(string name)
		{
			return new ChildValue(name);
		}
	}
}