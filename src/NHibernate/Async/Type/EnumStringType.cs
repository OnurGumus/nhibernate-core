using System;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class EnumStringType : AbstractEnumType
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name = "cached"></param>
		/// <param name = "session"></param>
		/// <param name = "owner"></param>
		/// <returns></returns>
		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			if (cached == null)
			{
				return null;
			}
			else
			{
				return GetInstance(cached);
			}
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			return (value == null) ? null : GetValue(value);
		}
	}
}