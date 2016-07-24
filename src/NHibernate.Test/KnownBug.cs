using NUnit.Framework;

namespace NHibernate.Test
{
	public class KnownBug
	{
		public static string Issue(string id)
		{
			return "Known bug " + id;
		}
	}
}