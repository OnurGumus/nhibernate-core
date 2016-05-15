#if NET_4_5
using System.Collections;
using System.Data.Common;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate
{
	/// <summary>
	/// Allows user code to inspect and/or change property values before they are written and after they
	/// are read from the database
	/// </summary>
	/// <remarks>
	///	<para>
	///	There might be a single instance of <c>IInterceptor</c> for a <c>SessionFactory</c>, or a new
	///	instance might be specified for each <c>ISession</c>. Whichever approach is used, the interceptor
	///	must be serializable if the <c>ISession</c> is to be serializable. This means that <c>SessionFactory</c>
	///	-scoped interceptors should implement <c>ReadResolve()</c>.
	///	</para>
	///	<para>
	///	The <c>ISession</c> may not be invoked from a callback (nor may a callback cause a collection or
	///	proxy to be lazily initialized).
	///	</para>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IInterceptor
	{
		/// <summary>
		/// Called after a flush that actually ends in execution of the SQL statements required to
		/// synchronize in-memory state with the database.
		/// </summary>
		/// <param name = "entities">The entities</param>
		Task PostFlushAsync(ICollection entities);
	}
}
#endif
