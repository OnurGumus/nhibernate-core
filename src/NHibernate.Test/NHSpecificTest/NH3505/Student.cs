using System;
using System.Collections;

namespace NHibernate.Test.NHSpecificTest.NH3505
{
	public partial class Student
	{
		public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Teacher Teacher { get; set; }
	}
}
