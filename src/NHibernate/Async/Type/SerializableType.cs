using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SerializableType : MutableType
	{
		public override async Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner)
		{
			return (cached == null) ? null : FromBytes((byte[])cached);
		}

		public override async Task<object> DisassembleAsync(object value, ISessionImplementor session, object owner)
		{
			return (value == null) ? null : ToBytes(value);
		}

		public override async Task<int> GetHashCodeAsync(Object x, EntityMode entityMode)
		{
			return await (binaryType.GetHashCodeAsync(ToBytes(x), entityMode));
		}
	}
}