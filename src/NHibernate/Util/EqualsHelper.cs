namespace NHibernate.Util
{
	public static partial class EqualsHelper
	{
		public new static bool Equals(object x, object y)
		{
			return x == y || (x != null && y != null && x.Equals(y));
		}
	}
}
