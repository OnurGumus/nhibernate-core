using System;

namespace NHibernate.Test.NHSpecificTest.NH1483
{
	public abstract partial class BaseClass
	{
		private Guid _id;

		public Guid Id
		{
			get { return _id; }
		}
	}
}