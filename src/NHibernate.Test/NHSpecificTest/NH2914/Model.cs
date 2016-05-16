using System;

namespace NHibernate.Test.NHSpecificTest.NH2914
{
	public partial class Entity
	{
		public virtual Guid Id { get; set; }
		public virtual DateTime CreationTime { get; set; }
	}
}
