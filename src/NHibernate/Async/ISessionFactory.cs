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
