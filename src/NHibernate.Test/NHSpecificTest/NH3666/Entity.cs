using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NHibernate.Test.NHSpecificTest.NH3666
{
	public partial class Entity
	{
		public virtual int Id { get; set; }
		public virtual string Property { get; set; }
	}
}
