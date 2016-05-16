namespace NHibernate.Test.IdTest
{
	public partial class Product
	{
		private string name;

		public virtual string Name
		{
			get { return name; }
			set { name = value; }
		}
	}
}