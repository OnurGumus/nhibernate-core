using System.Collections.Generic;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that returns the current identifier
	/// assigned to an instance.
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>&lt;generator class="assigned" /&gt;</code>
	/// </p>
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Assigned : IIdentifierGenerator, IConfigurable
	{
		/// <summary>
		/// Generates a new identifier by getting the value of the identifier
		/// for the <c>obj</c> parameter.
		/// </summary>
		/// <param name = "session">The <see cref = "ISessionImplementor"/> this id is being generated in.</param>
		/// <param name = "obj">The entity for which the id is being generated.</param>
		/// <returns>The value that was assigned to the mapped <c>id</c>'s property.</returns>
		/// <exception cref = "IdentifierGenerationException">
		/// Thrown when a <see cref = "IPersistentCollection"/> is passed in as the <c>obj</c> or
		/// if the identifier of <c>obj</c> is null.
		/// </exception>
		public async Task<object> GenerateAsync(ISessionImplementor session, object obj)
		{
			if (obj is IPersistentCollection)
			{
				throw new IdentifierGenerationException("Illegal use of assigned id generation for a toplevel collection");
			}

			object id = await (session.GetEntityPersister(entityName, obj).GetIdentifierAsync(obj, session.EntityMode));
			if (id == null)
			{
				throw new IdentifierGenerationException("ids for this class must be manually assigned before calling save(): " + obj.GetType().FullName);
			}

			return id;
		}
	}
}