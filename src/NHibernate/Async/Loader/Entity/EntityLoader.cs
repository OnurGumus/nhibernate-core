#if NET_4_5
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using NHibernate.Type;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Loader.Entity
{
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
