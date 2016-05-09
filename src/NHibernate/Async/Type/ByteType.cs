using System;
using System.Collections;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ByteType : PrimitiveType, IDiscriminatorType, IVersionType
	{
		public virtual async Task<object> SeedAsync(ISessionImplementor session)
		{
			return ZERO;
		}

		public virtual async Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return (byte)((byte)current + 1);
		}
	}
}