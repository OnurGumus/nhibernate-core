using System;

namespace NHibernate.Test.NHSpecificTest.NH1715
{
	public partial class ClassA
	{
		public virtual int Id { get; set; }
		public virtual TimeSpan Time { get; set; }
	}
}