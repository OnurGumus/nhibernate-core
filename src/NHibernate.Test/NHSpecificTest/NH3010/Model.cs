namespace NHibernate.Test.NHSpecificTest.NH3010
{
	using System;
	using System.Collections.Generic;

	public partial class Parent
	{
		public Parent()
		{
			Childs = new List<Child>();
		}

		public virtual Guid Id { get; set; }
		public virtual IList<Child> Childs { get; set; }
	}

	public partial class Child
	{
		public virtual Guid Id { get; set; }
	}
}
