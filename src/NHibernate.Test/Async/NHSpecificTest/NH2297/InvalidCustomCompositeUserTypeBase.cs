#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.UserTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH2297
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class InvalidCustomCompositeUserTypeBase : ICompositeUserType
	{
		public Task<object> NullSafeGetAsync(DbDataReader rs, String[] names, NHibernate.Engine.ISessionImplementor session, Object owner)
		{
			return NHibernateUtil.String.NullSafeGetAsync(rs, names[0], session, owner);
		}
	}
}
#endif
