using System;

namespace NHibernate.Test.NHSpecificTest.NH525
{
	public abstract partial class AbstractBase
	{
		private int id;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public abstract int AbstractMethod();
	}

	public partial class NonAbstract : AbstractBase
	{
		public const int AbstractMethodResult = 10;

		public override int AbstractMethod()
		{
			return AbstractMethodResult;
		}
	}
}