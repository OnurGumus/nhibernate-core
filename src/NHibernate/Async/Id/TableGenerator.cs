using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using NHibernate.AdoNet.Util;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that uses a database table to store the last
	/// generated value.
	/// </summary>
	/// <remarks>
	/// <p>
	/// It is not intended that applications use this strategy directly. However,
	/// it may be used to build other (efficient) strategies. The return type is
	/// <c>System.Int32</c>
	/// </p>
	/// <p>
	/// The hi value MUST be fetched in a separate transaction to the <c>ISession</c>
	/// transaction so the generator must be able to obtain a new connection and commit it.
	/// Hence this implementation may not be used when the user is supplying connections.
	/// </p>
	/// <p>
	/// The mapping parameters <c>table</c> and <c>column</c> are required.
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TableGenerator : TransactionHelper, IPersistentIdentifierGenerator, IConfigurable
	{
		/// <summary>
		/// Generate a <see cref = "short "/>, <see cref = "int "/>, or <see cref = "long "/> 
		/// for the identifier by selecting and updating a value in a table.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier as a <see cref = "short "/>, <see cref = "int "/>, or <see cref = "long "/>.</returns>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public virtual async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			// This has to be done using a different connection to the containing
			// transaction becase the new hi value must remain valid even if the
			// containing transaction rolls back.
			return DoWorkInNewTransaction(session);
		}
	}
}