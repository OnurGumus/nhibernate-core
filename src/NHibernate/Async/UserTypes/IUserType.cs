#if NET_4_5
using System.Data.Common;
using NHibernate.SqlTypes;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.UserTypes
{
	/// <summary>
	/// The interface to be implemented by user-defined types.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The interface abstracts user code from future changes to the <see cref = "Type.IType"/> interface,
	/// simplifies the implementation of custom types and hides certain "internal interfaces from
	/// user code.
	/// </para>
	/// <para>
	/// Implementers must be immutable and must declare a public default constructor.
	/// </para>
	/// <para>
	/// The actual class mapped by a <c>IUserType</c> may be just about anything. However, if it is to
	/// be cacheble by a persistent cache, it must be serializable.
	/// </para>
	/// <para>
	/// Alternatively, custom types could implement <see cref = "Type.IType"/> directly or extend one of the
	/// abstract classes in <c>NHibernate.Type</c>. This approach risks future incompatible changes
	/// to classes or interfaces in the package.
	/// </para>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IUserType
	{
		/// <summary>
		/// Retrieve an instance of the mapped class from a JDBC resultset.
		/// Implementors should handle possibility of null values.
		/// </summary>
		/// <param name = "rs">a DbDataReader</param>
		/// <param name = "names">column names</param>
		/// <param name = "owner">the containing entity</param>
		/// <returns></returns>
		/// <exception cref = "HibernateException">HibernateException</exception>
		 //		/// <exception cref="SQLException">SQLException</exception>
		Task<object> NullSafeGetAsync(DbDataReader rs, string[] names, object owner);
		/// <summary>
		/// Write an instance of the mapped class to a prepared statement.
		/// Implementors should handle possibility of null values.
		/// A multi-column type should be written to parameters starting from index.
		/// </summary>
		/// <param name = "cmd">a DbCommand</param>
		/// <param name = "value">the object to write</param>
		/// <param name = "index">command parameter index</param>
		/// <exception cref = "HibernateException">HibernateException</exception>
		 //		/// <exception cref="SQLException">SQLException</exception>
		Task NullSafeSetAsync(DbCommand cmd, object value, int index);
	}
}
#endif
