using System;

namespace NHibernate.Test.NHSpecificTest.NH296
{
	public partial class Stock : Product
	{
		private int _property;

		public int Property
		{
			get { return _property; }
			set { _property = value; }
		}
	}
}