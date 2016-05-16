using System;

namespace NHibernate.Test.Subclass.EnumDiscriminator
{
	public partial class Foo
	{
		private Int64 id;

		public long Id
		{
			get { return id; }
			set { id = value; }
		}
	}
}