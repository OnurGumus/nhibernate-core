using System;
using System.Collections;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeSpanType : PrimitiveType, IVersionType, ILiteralType
	{
		/// <summary></summary>
		public virtual async Task<object> SeedAsync(ISessionImplementor session)
		{
			return new TimeSpan(DateTime.Now.Ticks);
		}

		public async Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return await (SeedAsync(session));
		}
	}
}