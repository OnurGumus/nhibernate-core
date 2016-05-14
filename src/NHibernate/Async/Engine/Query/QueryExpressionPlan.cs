#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Hql;
using NHibernate.Linq;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Engine.Query
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryExpressionPlan : HQLQueryPlan, IQueryExpressionPlan
	{
		protected static Task<IQueryTranslator[]> CreateTranslatorsAsync(IQueryExpression queryExpression, string collectionRole, bool shallow, IDictionary<string, IFilter> enabledFilters, ISessionFactoryImplementor factory)
		{
			return factory.Settings.QueryTranslatorFactory.CreateQueryTranslatorsAsync(queryExpression, collectionRole, shallow, enabledFilters, factory);
		}
	}
}
#endif
