#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Hql
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial interface IQueryTranslatorFactory
	{
		/// <summary>
		/// Construct a <see cref = "NHibernate.Hql.IQueryTranslator"/> instance 
		/// capable of translating an HQL query string.
		/// </summary>
		/// <param name = "queryString">The query string to be translated</param>
		/// <param name = "collectionRole"></param>
		/// <param name = "shallow"></param>
		/// <param name = "filters">Currently enabled filters</param>
		/// <param name = "factory">The session factory</param>
		/// <returns>An appropriate translator.</returns>
		[Obsolete("Use overload with IQueryExpression")]
		Task<IQueryTranslator[]> CreateQueryTranslatorsAsync(string queryString, string collectionRole, bool shallow, IDictionary<string, IFilter> filters, ISessionFactoryImplementor factory);
		/// <summary>
		/// Construct a <see cref = "NHibernate.Hql.IQueryTranslator"/> instance 
		/// capable of translating a Linq expression.
		/// </summary>
		/// <param name = "queryExpression">The query expression to be translated</param>
		/// <param name = "collectionRole"></param>
		/// <param name = "shallow"></param>
		/// <param name = "filters">Currently enabled filters</param>
		/// <param name = "factory">The session factory</param>
		/// <returns>An appropriate translator.</returns>
		Task<IQueryTranslator[]> CreateQueryTranslatorsAsync(IQueryExpression queryExpression, string collectionRole, bool shallow, IDictionary<string, IFilter> filters, ISessionFactoryImplementor factory);
	}
}
#endif
