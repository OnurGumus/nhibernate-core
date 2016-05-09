using System;
using System.Data;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTime2Type : DateTimeType
	{
		public override async Task<object> NextAsync(object current, Engine.ISessionImplementor session)
		{
			return await (SeedAsync(session));
		}
	}
}