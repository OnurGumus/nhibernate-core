using System;

namespace NHibernate.Test.NHSpecificTest.NH266
{
	public partial class B : A
	{
		private long _number;

		public long Number
		{
			get { return _number; }
			set { _number = value; }
		}
	}
}