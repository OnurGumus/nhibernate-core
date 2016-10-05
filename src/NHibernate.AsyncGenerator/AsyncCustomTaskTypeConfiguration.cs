using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AsyncGenerator
{
	public class AsyncCustomTaskTypeConfiguration
	{
		public string TypeName { get; set; }

		public string Namespace { get; set; }

		public bool HasCompletedTask { get; set; }

		public bool HasFromException { get; set; }
	}
}
