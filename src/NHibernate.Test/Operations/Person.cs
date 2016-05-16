namespace NHibernate.Test.Operations
{
	public partial class Person
	{
		public virtual long Id { get; set; }
		public virtual string Name { get; set; }
		public virtual Address Address { get; set; }
		public virtual PersonalDetails Details { get; set; }
	}
}