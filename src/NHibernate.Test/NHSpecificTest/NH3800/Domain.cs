using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH3800
{
	public partial class Project
	{
		public Project()
		{
			Components = new List<Component>();
		}

		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual IList<Component> Components { get; set; }
	}

	public partial class Component
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
		public virtual Project Project { get; set; }
	}

	public partial class TimeRecord
	{
		public TimeRecord()
		{
			Components = new List<Component>();
			Tags = new List<Tag>();
		}

		public virtual Guid Id { get; set; }
		public virtual double TimeInHours { get; set; }
		public virtual Project Project { get; set; }
		public virtual IList<Component> Components { get; set; }
		public virtual IList<Tag> Tags { get; set; }

	}

	public partial class Tag
	{
		public virtual Guid Id { get; set; }
		public virtual string Name { get; set; }
	}
}
