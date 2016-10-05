using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.AsyncGenerator
{
	public class AsyncLockConfiguration
	{
		public string TypeName { get; set; }

		public string MethodName { get; set; }

		public string FieldName { get; set; }

		public string Namespace { get; set; }
	}
}
