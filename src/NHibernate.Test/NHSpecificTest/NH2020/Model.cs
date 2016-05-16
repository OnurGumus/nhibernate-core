namespace NHibernate.Test.NHSpecificTest.NH2020
{
	public partial class One
	{
		public virtual long Id { get; set; }
	}

	public partial class Many
	{
		public virtual long Id { get; set; }
		public virtual One One { get; set; }
	}
}
