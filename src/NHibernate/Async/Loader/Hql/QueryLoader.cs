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
		public async Task<IList> ListAsync(ISessionImplementor session, QueryParameters queryParameters)
		{
			CheckQuery(queryParameters);
			return await (ListAsync(session, queryParameters, _queryTranslator.QuerySpaces, _queryReturnTypes));
		}

		protected override async Task<object> GetResultColumnOrRowAsync(object[] row, IResultTransformer resultTransformer, IDataReader rs, ISessionImplementor session)
		{
			Object[] resultRow = await (GetResultRowAsync(row, rs, session));
			bool hasTransform = HasSelectNew || resultTransformer != null;
			return (!hasTransform && resultRow.Length == 1 ? resultRow[0] : resultRow);
		}

		protected override async Task<object[]> GetResultRowAsync(object[] row, IDataReader rs, ISessionImplementor session)
		{
			object[] resultRow;
			if (_hasScalars)
			{
				string[][] scalarColumns = _scalarColumnNames;
				int queryCols = _queryReturnTypes.Length;
				resultRow = new object[queryCols];
				for (int i = 0; i < queryCols; i++)
				{
					resultRow[i] = await (_queryReturnTypes[i].NullSafeGetAsync(rs, scalarColumns[i], session, null));
				}
			}
			else
			{
				resultRow = ToResultRow(row);
			}

			return resultRow;
		}

		internal async Task<IEnumerable> GetEnumerableAsync(QueryParameters queryParameters, IEventSource session)
		{
			CheckQuery(queryParameters);
			bool statsEnabled = session.Factory.Statistics.IsStatisticsEnabled;
			var stopWath = new Stopwatch();
			if (statsEnabled)
			{
				stopWath.Start();
			}

			IDbCommand cmd = await (PrepareQueryCommandAsync(queryParameters, false, session));
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