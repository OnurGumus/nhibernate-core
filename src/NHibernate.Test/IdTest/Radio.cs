namespace NHibernate.Test.IdTest
{
	public partial class Radio
	{
		private int id;
		private string frequency;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual string Frequency
		{
			get { return frequency; }
			set { frequency = value; }
		}
	}
}
