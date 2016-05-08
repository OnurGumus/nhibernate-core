using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using NHibernate.Engine;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Impl;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using IQueryable = NHibernate.Persister.Entity.IQueryable;
using System.Threading.Tasks;

namespace NHibernate.Loader.Hql
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryLoader : BasicLoader
	{
		internal async Task<IEnumerable> GetEnumerableAsync(QueryParameters queryParameters, IEventSource session)
		{
			CheckQuery(queryParameters);
			bool statsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
			var stopWath = new Stopwatch();
			if (statsEnabled)
			{
				stopWath.Start();
			}

			IDbCommand cmd = PrepareQueryCommand(queryParameters, false, session);
			// This IDataReader is disposed of in EnumerableImpl.Dispose
			IDataReader rs = await (GetResultSetAsync(cmd, queryParameters.HasAutoDiscoverScalarTypes, false, queryParameters.RowSelection, session));
			HolderInstantiator hi = HolderInstantiator.GetHolderInstantiator(_selectNewTransformer, queryParameters.ResultTransformer, _queryReturnAliases);
			IEnumerable result = new EnumerableImpl(rs, cmd, session, queryParameters.IsReadOnly(session), _queryTranslator.ReturnTypes, _queryTranslator.GetColumnNames(), queryParameters.RowSelection, hi);
			if (statsEnabled)
			{
				stopWath.Stop();
				session.Factory.StatisticsImplementor.QueryExecuted("HQL: " + _queryTranslator.QueryString, 0, stopWath.Elapsed);
				// NH: Different behavior (H3.2 use QueryLoader in AST parser) we need statistic for orginal query too.
				// probably we have a bug some where else for statistic RowCount
				session.Factory.StatisticsImplementor.QueryExecuted(QueryIdentifier, 0, stopWath.Elapsed);
			}

			return result;
		}
	}
}