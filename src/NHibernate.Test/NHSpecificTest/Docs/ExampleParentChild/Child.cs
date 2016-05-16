using System;

namespace NHibernate.Test.NHSpecificTest.Docs.ExampleParentChild
{
	public partial class Child
	{
		private long _id;
		private Parent _parent;

		public Child()
		{
		}

		public long Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public Parent Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}
	}
}