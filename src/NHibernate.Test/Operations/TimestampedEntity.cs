using System;

namespace NHibernate.Test.Operations
{
	public partial class TimestampedEntity
	{
		public virtual string Id { get; set; }
		public virtual string Name { get; set; }
		public virtual DateTime Timestamp { get; set; }
	}
}