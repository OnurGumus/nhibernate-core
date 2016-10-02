#if NET_4_5
using System;
using System.Collections;
using System.Data.Common;
using System.Text;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Type
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractBinaryType : MutableType, IVersionType, IComparer
	{
		//      Note : simply returns null for seed() and next() as the only known
		//      application of binary types for versioning is for use with the
		//      TIMESTAMP datatype supported by Sybase and SQL Server, which
		//      are completely db-generated values...
		public Task<object> NextAsync(object current, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Next(current, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		public Task<object> SeedAsync(ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(Seed(session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
