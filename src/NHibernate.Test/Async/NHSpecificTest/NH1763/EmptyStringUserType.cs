#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1763
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EmptyStringUserType : ICompositeUserType
	{
		public Task<object> NullSafeGetAsync(DbDataReader rs, String[] names, NHibernate.Engine.ISessionImplementor session, Object owner)
		{
			return NHibernateUtil.String.NullSafeGetAsync(rs, names[0], session, owner);
		}

		public async Task NullSafeSetAsync(DbCommand st, Object value, int index, bool[] settable, NHibernate.Engine.ISessionImplementor session)
		{
			if (settable[0])
			{
				string str = null;
				if (value != null)
					str = value.ToString().Trim();
				if (str == String.Empty)
				{
					str = null;
					await (NHibernateUtil.String.NullSafeSetAsync(st, str, index, session));
				}
				else
					await (NHibernateUtil.String.NullSafeSetAsync(st, value, index, session));
			}
		}
	}
}
#endif
