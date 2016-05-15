#if NET_4_5
using System;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.UserTypes
{
	/// <summary>
	/// A UserType that may be dereferenced in a query.
	/// This interface allows a custom type to define "properties".
	/// These need not necessarily correspond to physical .NET style properties.
	///
	/// A ICompositeUserType may be used in almost every way
	/// that a component may be used. It may even contain many-to-one
	/// associations.
	///
	/// Implementors must be immutable and must declare a public
	/// default constructor.
	///
	/// Unlike UserType, cacheability does not depend upon
	/// serializability. Instead, Assemble() and
	/// Disassemble() provide conversion to/from a cacheable
	/// representation.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ICompositeUserType
	{
		/// <summary>
		/// Retrieve an instance of the mapped class from a DbDataReader. Implementors
		/// should handle possibility of null values.
		/// </summary>
		/// <param name = "dr">DbDataReader</param>
		/// <param name = "names">the column names</param>
		/// <param name = "session"></param>
		/// <param name = "owner">the containing entity</param>
		/// <returns></returns>
		Task<object> NullSafeGetAsync(DbDataReader dr, string[] names, ISessionImplementor session, object owner);
		/// <summary>
		/// Write an instance of the mapped class to a prepared statement.
		/// Implementors should handle possibility of null values.
		/// A multi-column type should be written to parameters starting from index.
		/// If a property is not settable, skip it and don't increment the index.
		/// </summary>
		/// <param name = "cmd"></param>
		/// <param name = "value"></param>
		/// <param name = "index"></param>
		/// <param name = "settable"></param>
		/// <param name = "session"></param>
		Task NullSafeSetAsync(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session);
	}
}

namespace NHibernate.UserTypes
{
	/// <summary>
	/// A UserType that may be dereferenced in a query.
	/// This interface allows a custom type to define "properties".
	/// These need not necessarily correspond to physical .NET style properties.
	///
	/// A ICompositeUserType may be used in almost every way
	/// that a component may be used. It may even contain many-to-one
	/// associations.
	///
	/// Implementors must be immutable and must declare a public
	/// default constructor.
	///
	/// Unlike UserType, cacheability does not depend upon
	/// serializability. Instead, Assemble() and
	/// Disassemble() provide conversion to/from a cacheable
	/// representation.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface ICompositeUserType
	{
		/// <summary>
		/// Retrieve an instance of the mapped class from a DbDataReader. Implementors
		/// should handle possibility of null values.
		/// </summary>
		/// <param name = "dr">DbDataReader</param>
		/// <param name = "names">the column names</param>
		/// <param name = "session"></param>
		/// <param name = "owner">the containing entity</param>
		/// <returns></returns>
		Task<object> NullSafeGetAsync(DbDataReader dr, string[] names, ISessionImplementor session, object owner);
		/// <summary>
		/// Write an instance of the mapped class to a prepared statement.
		/// Implementors should handle possibility of null values.
		/// A multi-column type should be written to parameters starting from index.
		/// If a property is not settable, skip it and don't increment the index.
		/// </summary>
		/// <param name = "cmd"></param>
		/// <param name = "value"></param>
		/// <param name = "index"></param>
		/// <param name = "settable"></param>
		/// <param name = "session"></param>
		Task NullSafeSetAsync(DbCommand cmd, object value, int index, bool[] settable, ISessionImplementor session);
		/// <summary>
		/// Transform the object into its cacheable representation.
		/// At the very least this method should perform a deep copy.
		/// That may not be enough for some implementations, method should perform a deep copy. That may not be enough for some implementations, however; for example, associations must be cached as identifier values. (optional operation)
		/// </summary>
		/// <param name = "value">the object to be cached</param>
		/// <param name = "session"></param>
		/// <returns></returns>
		Task<object> DisassembleAsync(object value, ISessionImplementor session);
		/// <summary>
		/// Reconstruct an object from the cacheable representation.
		/// At the very least this method should perform a deep copy. (optional operation)
		/// </summary>
		/// <param name = "cached">the object to be cached</param>
		/// <param name = "session"></param>
		/// <param name = "owner"></param>
		/// <returns></returns>
		Task<object> AssembleAsync(object cached, ISessionImplementor session, object owner);
		/// <summary>
		/// During merge, replace the existing (target) value in the entity we are merging to
		/// with a new (original) value from the detached entity we are merging. For immutable
		/// objects, or null values, it is safe to simply return the first parameter. For
		/// mutable objects, it is safe to return a copy of the first parameter. However, since
		/// composite user types often define component values, it might make sense to recursively
		/// replace component values in the target object.
		/// </summary>
		Task<object> ReplaceAsync(object original, object target, ISessionImplementor session, object owner);
	}
}
#endif
