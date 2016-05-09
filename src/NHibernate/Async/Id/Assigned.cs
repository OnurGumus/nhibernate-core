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