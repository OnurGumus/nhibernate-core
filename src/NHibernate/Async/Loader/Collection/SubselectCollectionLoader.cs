#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Loader.Collection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SubselectCollectionLoader : BasicCollectionLoader
	{
		public override Task InitializeAsync(object id, ISessionImplementor session)
		{
			return LoadCollectionSubselectAsync(session, keys, values, types, namedParameters, KeyType);
		}
	}
}
#endif
