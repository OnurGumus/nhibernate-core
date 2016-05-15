#if NET_4_5
using System;
using System.Collections;
using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Transaction;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1054
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DummyTransactionFactory : ITransactionFactory
	{
		public Task ExecuteWorkInIsolationAsync(ISessionImplementor session, IIsolatedWork work, bool transacted)
		{
			try
			{
				ExecuteWorkInIsolation(session, work, transacted);
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
