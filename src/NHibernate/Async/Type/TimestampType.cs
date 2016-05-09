using System;
using System.Collections;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimestampType : PrimitiveType, IVersionType, ILiteralType
	{
		public virtual async Task<object> SeedAsync(ISessionImplementor session)
		{
			if (session == null)
			{
				return DateTime.Now;
			}

			return Round(DateTime.Now, session.Factory.Dialect.TimestampResolutionInTicks);
		}

		public async Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return await (SeedAsync(session));
		}
	}
}