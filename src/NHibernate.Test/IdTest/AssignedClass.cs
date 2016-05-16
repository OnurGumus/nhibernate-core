using System;
using System.Collections.Generic;

namespace NHibernate.Test.IdTest
{
	public partial class Parent
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public IList<Child> Children { get; set; }
	}

	public partial class Child
	{
		public string Id { get; set; }
		public Parent Parent { get; set; }
	}
}
