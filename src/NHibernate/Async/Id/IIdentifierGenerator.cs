#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Id
{
	/// <summary>
	/// The general contract between a class that generates unique
	/// identifiers and the <see cref = "ISession"/>.
	/// </summary>
	/// <remarks>
	/// <para>
	/// It is not intended that this interface ever be exposed to the 
	/// application.  It <b>is</b> intended that users implement this interface
	/// to provide custom identifier generation strategies.
	/// </para>
	/// <para>
	/// Implementors should provide a public default constructor.
	/// </para>
	/// <para>
	/// Implementations that accept configuration parameters should also
	/// implement <see cref = "IConfigurable"/>.
	/// </para>
	/// <para>
	/// Implementors <b>must</b> be threadsafe.
	/// </para>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IIdentifierGenerator
	{
		/// <summary>
		/// Generate a new identifier
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The new identifier</returns>
		Task<object> GenerateAsync(ISessionImplementor session, object obj);
	}
}
#endif
