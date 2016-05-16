namespace NHibernate.Test.NHSpecificTest.NH1654
{
	public partial class Employee
	{
		public virtual int Id { get; set; }

		public virtual string FirstName { get; set; }

		public virtual string FirstNameFormula { get; set; }
	}
}