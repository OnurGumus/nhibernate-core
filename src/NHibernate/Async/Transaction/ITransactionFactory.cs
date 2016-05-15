#if NET_4_5
using System.Collections;
using System.Transactions;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;
using NHibernate.Engine.Transaction;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Transaction
{
	/// <summary>
	/// An abstract factory for <see cref = "ITransaction"/> instances.
	/// Concrete implementations are specified by <c>transaction.factory_class</c> 
	/// configuration property.
	/// 
	/// Implementors must be threadsafe and should declare a public default constructor. 
	/// <seealso cref = "ITransactionContext"/>
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ITransactionFactory
	{
		Task ExecuteWorkInIsolationAsync(ISessionImplementor session, IIsolatedWork work, bool transacted);
	}
}
#endif
