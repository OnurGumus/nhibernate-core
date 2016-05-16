namespace NHibernate.Test.Generatedkeys.Seqidentity
{
	public partial class MyEntity
	{
		private int id;
		private string name;

		public virtual int Id
		{
			get { return id; }
		}

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}