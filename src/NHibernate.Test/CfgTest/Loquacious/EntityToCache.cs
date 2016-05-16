using System.Collections.Generic;

namespace NHibernate.Test.CfgTest.Loquacious
{
	public partial class EntityToCache
	{
		public string Name { get; set; }
		public IList<string> Elements { get; set; }
		public AnotherEntity Relation { get; set; }
	}

	public partial class AnotherEntity
	{
		public IList<string> Elements { get; set; }
	}
}