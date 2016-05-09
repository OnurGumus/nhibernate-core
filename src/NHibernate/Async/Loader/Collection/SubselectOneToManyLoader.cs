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
	/// Implements subselect fetching for a one to many association
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SubselectOneToManyLoader : OneToManyLoader
	{
		public override async Task InitializeAsync(object id, ISessionImplementor session)
		{
			await (LoadCollectionSubselectAsync(session, keys, values, types, namedParameters, KeyType));
		}
	}
}