using System;
using System.Data;
using NHibernate.SqlTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeType : PrimitiveType, IIdentifierType, ILiteralType
	{
		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			DateTime date = (DateTime)x;
			int hashCode = 1;
			unchecked
			{
				hashCode = 31 * hashCode + date.Second;
				hashCode = 31 * hashCode + date.Minute;
				hashCode = 31 * hashCode + date.Hour;
			}

			return hashCode;
		}
	}
}