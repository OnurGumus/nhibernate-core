using System;
using System.Collections;
using System.Threading.Tasks;
using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using NHibernate.Transaction;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1054
{
	public class DummyTransactionFactory : ITransactionFactory
	{
		public void Configure(IDictionary props)
		{
		}

		public ITransaction CreateTransaction(ISessionImplementor session)
		{
			throw new NotImplementedException();
		}

		public void EnlistInDistributedTransactionIfNeeded(ISessionImplementor session)
		{
			throw new NotImplementedException();
		}

		public bool IsInDistributedActiveTransaction(ISessionImplementor session)
		{
			return false;
		}

		public Task ExecuteWorkInIsolation(ISessionImplementor session, IIsolatedWork work, bool transacted)
		{
			return TaskHelper.FromException<bool>(new NotImplementedException());
		}
	}
}
