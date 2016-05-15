#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Linq;
using NHibernate.Mapping.ByCode;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2839
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class MyBooleanType : IEnhancedUserType
	{
		public async Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, object owner)
		{
			var ordinal = rs.GetOrdinal(names[0]);
			if (await (rs.IsDBNullAsync(ordinal)))
				return false;
			return rs.GetInt32(ordinal) == 1;
		}

		public Task NullSafeSetAsync(DbCommand cmd, object value, int index)
		{
			try
			{
				NullSafeSet(cmd, value, index);
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
