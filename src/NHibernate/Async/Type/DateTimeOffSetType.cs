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
	public partial class DateTimeOffsetType : PrimitiveType, IIdentifierType, ILiteralType, IVersionType
	{
		public async Task<object> SeedAsync(ISessionImplementor session)
		{
			return DateTimeOffset.Now;
		}

		public async Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return await (SeedAsync(session));
		}
	}
}