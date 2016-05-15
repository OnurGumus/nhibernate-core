#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Dialect;
using NHibernate.Exceptions;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Engine.Transaction
{
	/// <summary>
	/// Class which provides the isolation semantics required by
	/// an <see cref = "IIsolatedWork"/>.
	/// </summary>
	/// <remarks>
	/// <list type = "bullet">
	/// <listheader>
	///      <description>Processing comes in two flavors:</description>
	///  </listheader>
	///  <item>
	///      <term><see cref = "DoIsolatedWork"/> </term>
	///      <description>makes sure the work to be done is performed in a separate, distinct transaction</description>
	///  </item>
	///  <item>
	///      <term><see cref = "DoNonTransactedWork"/> </term>
	///      <description>makes sure the work to be done is performed outside the scope of any transaction</description>
	///  </item>
	/// </list>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Isolater
	{
		/// <summary> 
		/// Ensures that all processing actually performed by the given work will
		/// occur on a separate transaction. 
		/// </summary>
		/// <param name = "work">The work to be performed. </param>
		/// <param name = "session">The session from which this request is originating. </param>
		public static Task DoIsolatedWorkAsync(IIsolatedWork work, ISessionImplementor session)
		{
			return session.Factory.TransactionFactory.ExecuteWorkInIsolationAsync(session, work, true);
		}

		/// <summary> 
		/// Ensures that all processing actually performed by the given work will
		/// occur outside of a transaction. 
		/// </summary>
		/// <param name = "work">The work to be performed. </param>
		/// <param name = "session">The session from which this request is originating. </param>
		public static Task DoNonTransactedWorkAsync(IIsolatedWork work, ISessionImplementor session)
		{
			return session.Factory.TransactionFactory.ExecuteWorkInIsolationAsync(session, work, false);
		}
	}
}
#endif
