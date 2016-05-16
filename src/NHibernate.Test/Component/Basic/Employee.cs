using System;
using System.Collections.Generic;

namespace NHibernate.Test.Component.Basic
{
	public partial class Employee 
	{
		public virtual long Id { get; set; }
		
		public virtual Person Person { get; set; }
		
		public virtual DateTime HireDate { get; set; }
		
		public virtual OptionalComponent OptionalComponent { get; set; }
		
		public virtual ISet<Employee> DirectReports { get; set; }
	}
}