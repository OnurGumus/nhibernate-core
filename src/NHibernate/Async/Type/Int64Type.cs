using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Int64Type : PrimitiveType, IDiscriminatorType, IVersionType
	{
		public virtual async Task<object> SeedAsync(ISessionImplementor session)
		{
			return 1L;
		}

		public virtual async Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return (Int64)current + 1L;
		}
	}
}