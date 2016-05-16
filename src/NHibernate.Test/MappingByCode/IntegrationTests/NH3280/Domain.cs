namespace NHibernate.Test.MappingByCode.IntegrationTests.NH3280
{
	public partial class PersonDetail : PersonDetailBase
	{
		//public override Person Person { get; set; }
	}

	public partial class PersonDetailBase
	{
		public virtual string LastName { get; set; }
		public virtual Person Person { get; set; }
		public virtual int? PersonDetailId { get; set; }
	}

	public partial class Person : PersonBase
	{
	}

	public partial class PersonBase
	{
		public virtual string FirstName { get; set; }
		public virtual PersonDetail PersonDetail { get; set; }
		public virtual int? PersonId { get; set; }
	}
}
