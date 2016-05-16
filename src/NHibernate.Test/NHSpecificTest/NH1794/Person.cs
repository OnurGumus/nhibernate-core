namespace NHibernate.Test.NHSpecificTest.NH1794
{
	public partial class Person
	{
		public virtual int Id { get; set; }
		public virtual string Name { get; set; }
		public virtual Person Parent { get; set; }
	}
}