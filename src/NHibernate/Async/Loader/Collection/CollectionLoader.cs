using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;

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
		public virtual async Task InitializeAsync(object id, ISessionImplementor session)
		{
			await (LoadCollectionAsync(session, id, KeyType));
		}
	}
}