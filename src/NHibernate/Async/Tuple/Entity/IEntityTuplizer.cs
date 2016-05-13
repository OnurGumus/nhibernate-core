using System.Collections;
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Tuple.Entity
{
	/// <summary> 
	/// Defines further responsibilities regarding tuplization based on a mapped entity.
	/// </summary>
	/// <remarks>
	/// EntityTuplizer implementations should have the following constructor signature:
	/// (<see cref = "EntityMetamodel"/>, <see cref = "Mapping.PersistentClass"/>)
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IEntityTuplizer : ITuplizer
	{
		/// <summary> Create an entity instance initialized with the given identifier. </summary>
		/// <param name = "id">The identifier value for the entity to be instantiated. </param>
		/// <returns> The instantiated entity. </returns>
		Task<object> InstantiateAsync(object id);
		/// <summary> Extract the identifier value from the given entity. </summary>
		/// <param name = "entity">The entity from which to extract the identifier value. </param>
		/// <returns> The identifier value. </returns>
		Task<object> GetIdentifierAsync(object entity);
		/// <summary> 
		/// Inject the identifier value into the given entity.
		/// </summary>
		/// <param name = "entity">The entity to inject with the identifier value.</param>
		/// <param name = "id">The value to be injected as the identifier. </param>
		/// <remarks>Has no effect if the entity does not define an identifier property</remarks>
		Task SetIdentifierAsync(object entity, object id);
		/// <summary> 
		/// Inject the given identifier and version into the entity, in order to
		/// "roll back" to their original values. 
		/// </summary>
		/// <param name = "entity"></param>
		/// <param name = "currentId">The identifier value to inject into the entity. </param>
		/// <param name = "currentVersion">The version value to inject into the entity. </param>
		Task ResetIdentifierAsync(object entity, object currentId, object currentVersion);
		/// <summary> Extract the values of the insertable properties of the entity (including backrefs) </summary>
		/// <param name = "entity">The entity from which to extract. </param>
		/// <param name = "mergeMap">a map of instances being merged to merged instances </param>
		/// <param name = "session">The session in which the request is being made. </param>
		/// <returns> The insertable property values. </returns>
		Task<object[]> GetPropertyValuesToInsertAsync(object entity, IDictionary mergeMap, ISessionImplementor session);
	}
}