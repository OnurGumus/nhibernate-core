namespace NHibernate.Test.NHSpecificTest.NH2303
{
	public abstract partial class Actor
	{
		public virtual int Id { get; set; }
	}

	public partial class Person : Actor
	{
	}

	public abstract partial class Role : Actor
	{
		public virtual Person Performer { get; set; }
	}

	public partial class Developer : Role
	{
	}
}