namespace NHibernate.Test.LazyOneToOne
{
	public partial class Person
	{
		public virtual string Name { get; set; }
		public virtual Employee Employee { get; set; }
	}
}