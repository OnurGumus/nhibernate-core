using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Loader.Criteria;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Loader
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEntityJoinWalker : JoinWalker
	{
		protected virtual async Task InitAllAsync(SqlString whereString, SqlString orderByString, LockMode lockMode)
		{
			await (WalkEntityTreeAsync(persister, Alias));
			IList<OuterJoinableAssociation> allAssociations = new List<OuterJoinableAssociation>(associations);
			allAssociations.Add(new OuterJoinableAssociation(persister.EntityType, null, null, alias, JoinType.LeftOuterJoin, null, Factory, new CollectionHelper.EmptyMapClass<string, IFilter>()));
			InitPersisters(allAssociations, lockMode);
			InitStatementString(whereString, orderByString, lockMode);
		}

		protected async Task InitProjectionAsync(SqlString projectionString, SqlString whereString, SqlString orderByString, SqlString groupByString, SqlString havingString, IDictionary<string, IFilter> enabledFilters, LockMode lockMode)
		{
			await (WalkEntityTreeAsync(persister, Alias));
			Persisters = new ILoadable[0];
			InitStatementString(projectionString, whereString, orderByString, groupByString, havingString, lockMode);
		}
	}
}