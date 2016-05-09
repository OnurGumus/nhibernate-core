using System;
using System.Collections;
using System.Data;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeType : PrimitiveType, IIdentifierType, ILiteralType, IVersionType
	{
		public override async Task<int> GetHashCodeAsync(object x, EntityMode entityMode)
		{
			// Custom hash code implementation because DateTimeType is only accurate
			// up to seconds.
			DateTime date = (DateTime)x;
			int hashCode = 1;
			unchecked
			{
				hashCode = 31 * hashCode + date.Second;
				hashCode = 31 * hashCode + date.Minute;
				hashCode = 31 * hashCode + date.Hour;
				hashCode = 31 * hashCode + date.Day;
				hashCode = 31 * hashCode + date.Month;
				hashCode = 31 * hashCode + date.Year;
			}

			return hashCode;
		}

		public virtual async Task<object> SeedAsync(ISessionImplementor session)
		{
			return TimestampType.Round(DateTime.Now, TimeSpan.TicksPerSecond);
		}

		public virtual async Task<object> NextAsync(object current, ISessionImplementor session)
		{
			return await (SeedAsync(session));
		}
	}
}