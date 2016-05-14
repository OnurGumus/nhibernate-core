#if NET_4_5
using NHibernate.Engine;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Loader.Entity
{
	/// <summary>
	///  Loads entities for a <see cref = "NHibernate.Persister.Entity.IEntityPersister"/>
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IUniqueEntityLoader
	{
		/// <summary>
		/// Load an entity instance. If <c>OptionalObject</c> is supplied, load the entity
		/// state into the given (uninitialized) object
		/// </summary>
		Task<object> LoadAsync(object id, object optionalObject, ISessionImplementor session);
	}
}
#endif
