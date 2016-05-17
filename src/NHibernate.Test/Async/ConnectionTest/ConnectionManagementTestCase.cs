#if NET_4_5
using System;
using System.Collections;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.ConnectionTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class ConnectionManagementTestCase : TestCase
	{
		protected virtual Task ReleaseAsync(ISession session)
		{
			try
			{
				Release(session);
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
