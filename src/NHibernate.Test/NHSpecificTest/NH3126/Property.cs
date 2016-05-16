using System;

namespace NHibernate.Test.NHSpecificTest.NH3126
{
	public partial class Property
	{
		public virtual Guid Id { get; set; }

		public virtual string Name { get; set; }
	}
}