namespace NHibernate.Test.NHSpecificTest.NH479
{
	public partial class Main
	{
		private long id;
		private Aggregate aggregate;

		public long Id
		{
			get { return id; }
			set { id = value; }
		}

		public Aggregate Aggregate
		{
			get { return aggregate; }
			set { aggregate = value; }
		}
	}

	public partial class Aggregate
	{
		private long id;
		private Main main;

		public long Id
		{
			get { return id; }
			set { id = value; }
		}

		public Main Main
		{
			get { return main; }
			set { main = value; }
		}
	}
}