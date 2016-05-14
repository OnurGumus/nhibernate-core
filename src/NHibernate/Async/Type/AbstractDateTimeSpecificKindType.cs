#if NET_4_5
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractDateTimeSpecificKindType : DateTimeType
	{
		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			int hashCode = await (base.GetHashCodeAsync(x, entityMode));
			unchecked
			{
				hashCode = 31 * hashCode + ((DateTime)x).Kind.GetHashCode();
			}

			return hashCode;
		}
	}
}
#endif
