#if NET_4_5
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2234
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SimpleCustomType : IUserType
	{
		public async Task NullSafeSetAsync(DbCommand cmd, object value, int index)
		{
			if (value == null)
				await (NHibernateUtil.Int32.NullSafeSetAsync(cmd, null, index, null));
			else
				await (NHibernateUtil.Int32.NullSafeSetAsync(cmd, ((MyUsertype)value).Id, index, null));
		}

		public async Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, object owner)
		{
			int value = (int)await (NHibernateUtil.Int32.NullSafeGetAsync(rs, names[0], null, owner));
			return MyUserTypes.Find(value);
		}
	}
}
#endif
