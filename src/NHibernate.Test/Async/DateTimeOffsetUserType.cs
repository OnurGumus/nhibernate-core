#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;

namespace NHibernate.Test
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeOffsetUserType : IUserType, IParameterizedType
	{
		public async Task<object> NullSafeGetAsync(DbDataReader dr, string[] names, object owner)
		{
			var name = names[0];
			int index = dr.GetOrdinal(name);
			if (await (dr.IsDBNullAsync(index)))
			{
				return null;
			}

			try
			{
				DateTime storedTime;
				try
				{
					DateTime dbValue = Convert.ToDateTime(dr[index]);
					storedTime = new DateTime(dbValue.Year, dbValue.Month, dbValue.Day, dbValue.Hour, dbValue.Minute, dbValue.Second);
				}
				catch (Exception ex)
				{
					throw new FormatException(string.Format("Input string '{0}' was not in the correct format.", dr[index]), ex);
				}

				return new DateTimeOffset(storedTime, Offset);
			}
			catch (InvalidCastException ice)
			{
				throw new ADOException(string.Format("Could not cast the value in field {0} of type {1} to the Type {2}.  Please check to make sure that the mapping is correct and that your DataProvider supports this Data Type.", names[0], dr[index].GetType().Name, GetType().Name), ice);
			}
		}
	}
}
#endif
