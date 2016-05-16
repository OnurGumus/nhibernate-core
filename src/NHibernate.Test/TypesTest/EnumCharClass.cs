using System;
using NHibernate.Type;

namespace NHibernate.Test.TypesTest
{
	public partial class EnumCharClass
	{
		private int _id;
		private SampleCharEnum _enumValue;

		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public SampleCharEnum EnumValue
		{
			get { return _enumValue; }
			set { _enumValue = value; }
		}
	}

	public enum SampleCharEnum
	{
		On = 'N',
		Off = 'F',
		Dimmed = 'D'
	}


	public partial class EnumCharFoo
	{
		private Int32 id;

		public virtual Int32 Id
		{
			get { return id; }
			set { id = value; }
		}
	}

	public partial class EnumCharBar : EnumCharFoo {}

	public partial class EnumCharBaz
	{
		private Int32 id;
		private SampleCharEnum type;

		public virtual Int32 Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual SampleCharEnum Type
		{
			get { return type; }
			set { type = value; }
		}
	}
}