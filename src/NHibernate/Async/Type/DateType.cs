using System;
using System.Collections.Generic;
using System.Data;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateType : PrimitiveType, IIdentifierType, ILiteralType, IParameterizedType
	{
		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			DateTime date = (DateTime)x;
			int hashCode = 1;
			unchecked
			{
				hashCode = 31 * hashCode + date.Day;
				hashCode = 31 * hashCode + date.Month;
				hashCode = 31 * hashCode + date.Year;
			}

			return hashCode;
		}
	}
}