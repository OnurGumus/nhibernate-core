using System;

namespace NHibernate.Test.NHSpecificTest.NH712
{
	// Class without the default constructor
	public partial class Component
	{
		public Component(int x)
		{
		}
	}

	public partial class Container
	{
		private int id;
		private Component component;

		public virtual int Id
		{
			get { return id; }
			set { id = value; }
		}

		public virtual Component Component
		{
			get { return component; }
			set { component = value; }
		}
	}
}