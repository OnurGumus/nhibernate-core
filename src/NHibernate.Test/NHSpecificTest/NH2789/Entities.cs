using System;

namespace NHibernate.Test.NHSpecificTest.NH2789
{
	public partial class EntityWithAByteValue
	{
		public virtual Guid Id { get; protected set; }
		public virtual byte? ByteValue { get; set; }
	}
}