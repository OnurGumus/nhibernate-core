using System;
using System.Collections.Generic;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	using System.Runtime.Serialization;

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PersistentEnumType : AbstractEnumType
	{
		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			return cached == null ? null : GetInstance(cached);
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			return (value == null) ? null : GetValue(value);
		}
	}
}