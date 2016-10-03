#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Type;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Cache
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IQueryCache
	{
		Task<bool> PutAsync(QueryKey key, ICacheAssembler[] returnTypes, IList result, bool isNaturalKeyLookup, ISessionImplementor session);
		Task<IList> GetAsync(QueryKey key, ICacheAssembler[] returnTypes, bool isNaturalKeyLookup, ISet<string> spaces, ISessionImplementor session);
	}
}
#endif
