using System.Collections;
using NHibernate.Type;
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Metadata
{
	/// <summary>
	/// Exposes entity class metadata to the application
	/// </summary>
	/// <seealso cref = "NHibernate.ISessionFactory.GetClassMetadata(System.Type)"/>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IClassMetadata
	{
		/// <summary> Return the values of the mapped properties of the object</summary>
		Task<object[]> GetPropertyValuesToInsertAsync(object entity, IDictionary mergeMap, ISessionImplementor session);
		/// <summary>
		/// Create a class instance initialized with the given identifier
		/// </summary>
		Task<object> InstantiateAsync(object id, EntityMode entityMode);
		/// <summary>
		/// Get the identifier of an instance (throw an exception if no identifier property)
		/// </summary>
		Task<object> GetIdentifierAsync(object entity, EntityMode entityMode);
		/// <summary>
		/// Set the identifier of an instance (or do nothing if no identifier property)
		/// </summary>
		Task SetIdentifierAsync(object entity, object id, EntityMode entityMode);
	}
}