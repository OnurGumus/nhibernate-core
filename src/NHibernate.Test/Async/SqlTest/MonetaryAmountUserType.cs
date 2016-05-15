#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.SqlTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MonetaryAmountUserType : IUserType
	{
		public async Task<object> NullSafeGetAsync(DbDataReader resultSet, string[] names, object owner)
		{
			int index0 = resultSet.GetOrdinal(names[0]);
			int index1 = resultSet.GetOrdinal(names[1]);
			if (await (resultSet.IsDBNullAsync(index0)))
			{
				return null;
			}

			decimal value = resultSet.GetDecimal(index0);
			string cur = resultSet.GetString(index1);
			return new MonetaryAmount(value, cur);
		}

		public Task NullSafeSetAsync(DbCommand statement, object value, int index)
		{
			try
			{
				NullSafeSet(statement, value, index);
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
