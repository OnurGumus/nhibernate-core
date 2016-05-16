namespace NHibernate.Test.TypesTest
{
	public partial class CharClass
	{
		public int Id { get; set; }
		public virtual char NormalChar { get; set; }
		public virtual char? NullableChar { get; set; }
	}
}