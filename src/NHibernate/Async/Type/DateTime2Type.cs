#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTime2Type : DateTimeType
	{
		public override Task<object> NextAsync(object current, Engine.ISessionImplementor session)
		{
			return SeedAsync(session);
		}
	}
}
#endif
