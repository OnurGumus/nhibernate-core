using System.Collections.Generic;

namespace NHibernate.Test.NHSpecificTest.NH1069
{
	public partial class LazyE
	{
		public virtual string Name { get; set; }
		public virtual ISet<string> LazyC { get; set; }
	}
}