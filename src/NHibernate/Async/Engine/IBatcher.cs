#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Engine
{
	/// <summary>
	/// Manages <see cref = "DbCommand"/>s and <see cref = "DbDataReader"/>s 
	/// for an <see cref = "ISession"/>. 
	/// </summary>
	/// <remarks>
	/// <p>
	/// Abstracts ADO.NET batching to maintain the illusion that a single logical batch 
	/// exists for the whole session, even when batching is disabled.
	/// Provides transparent <c>DbCommand</c> caching.
	/// </p>
	/// <p>
	/// This will be useful once ADO.NET gets support for batching.  Until that point
	/// no code exists that will do batching, but this will provide a good point to do
	/// error checking and making sure the correct number of rows were affected.
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IBatcher : IDisposable
	{
		Task<DbDataReader> ExecuteReaderAsync(DbCommand cmd);
	}
}
#endif
