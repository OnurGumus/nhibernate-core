namespace NHibernate.Test.NHSpecificTest.NH952
{
	public partial class Item
	{
		private int uniqueId;

		public virtual int UniqueId
		{
			get { return uniqueId; }
			set { uniqueId = value; }
		}
	}
}