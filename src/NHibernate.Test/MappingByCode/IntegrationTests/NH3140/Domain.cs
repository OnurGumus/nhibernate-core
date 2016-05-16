using System;
using System.Collections.Generic;

namespace NHibernate.Test.MappingByCode.IntegrationTests.NH3140
{
	public partial class Foo
	{
		public virtual Guid Id { get; set; }
		public virtual ICollection<Bar> Bars { get; set; }
	}

	public partial class Bar
	{
		public virtual Guid Id { get; set; }
	}
}