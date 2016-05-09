namespace NHibernate.Dialect
{
	public partial class MsSqlAzure2008Dialect : MsSql2008Dialect
	{
		public override string PrimaryKeyString
		{
			get { return "primary key CLUSTERED"; }
		}
	}
}