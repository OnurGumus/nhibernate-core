#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Connection;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Stat;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	/// <summary>
	/// Creates <c>ISession</c>s.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Usually an application has a single <c>SessionFactory</c>. Threads servicing client requests
	/// obtain <c>ISession</c>s from the factory. Implementors must be threadsafe.
	/// </para>
	/// <para>
	/// <c>ISessionFactory</c>s are immutable. The behaviour of a <c>SessionFactory</c>
	/// is controlled by properties supplied at configuration time.
	/// These properties are defined on <c>Environment</c>
	/// </para>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ISessionFactory : IDisposable
	{
		/// <summary>
		/// Destroy this <c>SessionFactory</c> and release all resources 
		/// connection pools, etc). It is the responsibility of the application
		/// to ensure that there are no open <c>Session</c>s before calling
		/// <c>close()</c>. 
		/// </summary>
		Task CloseAsync();
	}
}
#endif
