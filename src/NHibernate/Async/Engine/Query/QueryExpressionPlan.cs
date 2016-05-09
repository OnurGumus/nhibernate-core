using System;
using System.Collections.Generic;
using NHibernate.Hql;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Engine.Query
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryExpressionPlan : HQLQueryPlan, IQueryExpressionPlan
	{
		protected static async Task<IQueryTranslator[]> CreateTranslatorsAsync(IQueryExpression queryExpression, string collectionRole, bool shallow, IDictionary<string, IFilter> enabledFilters, ISessionFactoryImplementor factory)
		{
			return await (factory.Settings.QueryTranslatorFactory.CreateQueryTranslatorsAsync(queryExpression, collectionRole, shallow, enabledFilters, factory));
		}
	}
}