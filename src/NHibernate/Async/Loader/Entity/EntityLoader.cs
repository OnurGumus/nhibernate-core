#if NET_4_5
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Loader.Entity
{
	/// <summary>
	/// Load an entity using outerjoin fetching to fetch associated entities.
	/// </summary>
	/// <remarks>
	/// The <see cref = "IEntityPersister"/> must implement <see cref = "ILoadable"/>. For other entities,
	/// create a customized subclass of <see cref = "Loader"/>.
	/// </remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityLoader : AbstractEntityLoader
	{
		public Task<object> LoadByUniqueKeyAsync(ISessionImplementor session, object key)
		{
			return LoadAsync(session, key, null, null);
		}
	}
}
#endif
