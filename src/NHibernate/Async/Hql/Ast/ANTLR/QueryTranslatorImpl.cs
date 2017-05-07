﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Antlr.Runtime;
using Antlr.Runtime.Tree;

using NHibernate.Engine;
using NHibernate.Engine.Query;
using NHibernate.Event;
using NHibernate.Hql.Ast.ANTLR.Exec;
using NHibernate.Hql.Ast.ANTLR.Tree;
using NHibernate.Hql.Ast.ANTLR.Util;
using NHibernate.Loader.Hql;
using NHibernate.Param;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using IQueryable = NHibernate.Persister.Entity.IQueryable;

namespace NHibernate.Hql.Ast.ANTLR
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class QueryTranslatorImpl : IFilterTranslator
	{

		public async Task<IList> ListAsync(ISessionImplementor session, QueryParameters queryParameters)
		{
			// Delegate to the QueryLoader...
			ErrorIfDML();
			var query = ( QueryNode ) _sqlAst;
			bool hasLimit = queryParameters.RowSelection != null && queryParameters.RowSelection.DefinesLimits;
			bool needsDistincting = ( query.GetSelectClause().IsDistinct || hasLimit ) && ContainsCollectionFetches;

			QueryParameters queryParametersToUse;

			if ( hasLimit && ContainsCollectionFetches ) 
			{
				log.Warn( "firstResult/maxResults specified with collection fetch; applying in memory!" );
				var selection = new RowSelection
											{
												FetchSize = queryParameters.RowSelection.FetchSize,
												Timeout = queryParameters.RowSelection.Timeout
											};
				queryParametersToUse = queryParameters.CreateCopyUsing( selection );
			}
			else 
			{
				queryParametersToUse = queryParameters;
			}

			IList results = await (_queryLoader.ListAsync(session, queryParametersToUse)).ConfigureAwait(false);

			if ( needsDistincting ) 
			{
				int includedCount = -1;
				// NOTE : firstRow is zero-based
				int first = !hasLimit || queryParameters.RowSelection.FirstRow == RowSelection.NoValue
							? 0
							: queryParameters.RowSelection.FirstRow;
				int max = !hasLimit || queryParameters.RowSelection.MaxRows == RowSelection.NoValue
							? -1
							: queryParameters.RowSelection.MaxRows;

				int size = results.Count;
				var tmp = new List<object>();
				var distinction = new IdentitySet();

				for ( int i = 0; i < size; i++ ) 
				{
					object result = results[i];
					if ( !distinction.Add(result ) ) 
					{
						continue;
					}
					includedCount++;
					if ( includedCount < first ) 
					{
						continue;
					}
					tmp.Add( result );
					// NOTE : ( max - 1 ) because first is zero-based while max is not...
					if ( max >= 0 && ( includedCount - first ) >= ( max - 1 ) ) 
					{
						break;
					}
				}

				results = tmp;
			}

			return results;
		}

		public Task<IEnumerable> GetEnumerableAsync(QueryParameters queryParameters, IEventSource session)
		{
			try
			{
				ErrorIfDML();
				return _queryLoader.GetEnumerableAsync(queryParameters, session);
			}
			catch (Exception ex)
			{
				return Task.FromException<IEnumerable>(ex);
			}
		}

		public Task<int> ExecuteUpdateAsync(QueryParameters queryParameters, ISessionImplementor session)
		{
			try
			{
				ErrorIfSelect();
				return _statementExecutor.ExecuteAsync(queryParameters, session);
			}
			catch (Exception ex)
			{
				return Task.FromException<int>(ex);
			}
		}
	}
}
