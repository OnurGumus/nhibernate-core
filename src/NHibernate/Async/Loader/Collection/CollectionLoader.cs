#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Loader.Collection
{
	/// <summary>
	/// Superclass for loaders that initialize collections
	/// <seealso cref = "OneToManyLoader"/>
	/// <seealso cref = "BasicCollectionLoader"/>
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionLoader : OuterJoinLoader, ICollectionInitializer
	{
		public virtual Task InitializeAsync(object id, ISessionImplementor session)
		{
			return LoadCollectionAsync(session, id, KeyType);
		}
	}
}
#endif
