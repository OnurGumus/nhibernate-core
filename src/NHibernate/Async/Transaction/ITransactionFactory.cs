#if NET_4_5
using System.Collections;
using System.Transactions;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Transaction
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ITransactionFactory
	{
		Task ExecuteWorkInIsolationAsync(ISessionImplementor session, IIsolatedWork work, bool transacted);
	}
}
#endif
