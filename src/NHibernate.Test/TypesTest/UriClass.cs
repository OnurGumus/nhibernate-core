using System;

namespace NHibernate.Test.TypesTest
{
	public partial class UriClass
	{
		public int Id { get; set; }
		public Uri Url { get; set; }
		public Uri AutoUri { get; set; }
	}
}