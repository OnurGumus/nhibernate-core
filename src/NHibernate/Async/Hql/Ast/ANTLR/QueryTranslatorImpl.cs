#if NET_4_5
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
using System.Threading.Tasks;

namespace NHibernate.Hql.Ast.ANTLR
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryTranslatorImpl : IFilterTranslator
	{
		/// <summary>
		/// Compile a "normal" query. This method may be called multiple
		/// times. Subsequent invocations are no-ops.
		/// </summary>
		/// <param name = "replacements">Defined query substitutions.</param>
		/// <param name = "shallow">Does this represent a shallow (scalar or entity-id) select?</param>
		public Task CompileAsync(IDictionary<string, string> replacements, bool shallow)
		{
			return DoCompileAsync(replacements, shallow, null);
		}

		/// <summary>
		/// Compile a filter. This method may be called multiple
		/// times. Subsequent invocations are no-ops.
		/// </summary>
		/// <param name = "collectionRole">the role name of the collection used as the basis for the filter.</param>
		/// <param name = "replacements">Defined query substitutions.</param>
		/// <param name = "shallow">Does this represent a shallow (scalar or entity-id) select?</param>
		public Task CompileAsync(string collectionRole, IDictionary<string, string> replacements, bool shallow)
		{
			return DoCompileAsync(replacements, shallow, collectionRole);
		}

		public async Task<IList> ListAsync(ISessionImplementor session, QueryParameters queryParameters)
		{
			// Delegate to the QueryLoader...
			ErrorIfDML();
			var query = (QueryNode)_sqlAst;
			bool hasLimit = queryParameters.RowSelection != null && queryParameters.RowSelection.DefinesLimits;
			bool needsDistincting = (query.GetSelectClause().IsDistinct || hasLimit) && ContainsCollectionFetches;
			QueryParameters queryParametersToUse;
			if (hasLimit && ContainsCollectionFetches)
			{
				log.Warn("firstResult/maxResults specified with collection fetch; applying in memory!");
				var selection = new RowSelection{FetchSize = queryParameters.RowSelection.FetchSize, Timeout = queryParameters.RowSelection.Timeout};
				queryParametersToUse = queryParameters.CreateCopyUsing(selection);
			}
			else
			{
				queryParametersToUse = queryParameters;
			}

			IList results = await (_queryLoader.ListAsync(session, queryParametersToUse));
			if (needsDistincting)
			{
				int includedCount = -1;
				// NOTE : firstRow is zero-based
				int first = !hasLimit || queryParameters.RowSelection.FirstRow == RowSelection.NoValue ? 0 : queryParameters.RowSelection.FirstRow;
				int max = !hasLimit || queryParameters.RowSelection.MaxRows == RowSelection.NoValue ? -1 : queryParameters.RowSelection.MaxRows;
				int size = results.Count;
				var tmp = new List<object>();
				var distinction = new IdentitySet();
				for (int i = 0; i < size; i++)
				{
					object result = results[i];
					if (!distinction.Add(result))
					{
						continue;
					}

					includedCount++;
					if (includedCount < first)
					{
						continue;
					}

					tmp.Add(result);
					// NOTE : ( max - 1 ) because first is zero-based while max is not...
					if (max >= 0 && (includedCount - first) >= (max - 1))
					{
						break;
					}
				}

				results = tmp;
			}

			return results;
		}

		public async Task<IEnumerable> GetEnumerableAsync(QueryParameters queryParameters, IEventSource session)
		{
			ErrorIfDML();
			return await (_queryLoader.GetEnumerableAsync(queryParameters, session));
		}

		public async Task<int> ExecuteUpdateAsync(QueryParameters queryParameters, ISessionImplementor session)
		{
			ErrorIfSelect();
			return await (_statementExecutor.ExecuteAsync(queryParameters, session));
		}

		/// <summary>
		/// Performs both filter and non-filter compiling.
		/// </summary>
		/// <param name = "replacements">Defined query substitutions.</param>
		/// <param name = "shallow">Does this represent a shallow (scalar or entity-id) select?</param>
		/// <param name = "collectionRole">the role name of the collection used as the basis for the filter, NULL if this is not a filter.</param>
		private async Task DoCompileAsync(IDictionary<string, string> replacements, bool shallow, String collectionRole)
		{
			// If the query is already compiled, skip the compilation.
			if (_compiled)
			{
				if (log.IsDebugEnabled)
				{
					log.Debug("compile() : The query is already compiled, skipping...");
				}

				return;
			}

			// Remember the parameters for the compilation.
			_tokenReplacements = replacements ?? new Dictionary<string, string>(1);
			_shallowQuery = shallow;
			try
			{
				// PHASE 1 : Analyze the HQL AST, and produce an SQL AST.
				var translator = await (AnalyzeAsync(collectionRole));
				_sqlAst = translator.SqlStatement;
				// at some point the generate phase needs to be moved out of here,
				// because a single object-level DML might spawn multiple SQL DML
				// command executions.
				//
				// Possible to just move the sql generation for dml stuff, but for
				// consistency-sake probably best to just move responsiblity for
				// the generation phase completely into the delegates
				// (QueryLoader/StatementExecutor) themselves.  Also, not sure why
				// QueryLoader currently even has a dependency on this at all; does
				// it need it?  Ideally like to see the walker itself given to the delegates directly...
				if (_sqlAst.NeedsExecutor)
				{
					_statementExecutor = BuildAppropriateStatementExecutor(_sqlAst);
				}
				else
				{
					// PHASE 2 : Generate the SQL.
					_generator = new HqlSqlGenerator(_sqlAst, _factory);
					_generator.Generate();
					_queryLoader = new QueryLoader(this, _factory, _sqlAst.Walker.SelectClause);
				}

				_compiled = true;
			}
			catch (QueryException qe)
			{
				qe.QueryString = _queryIdentifier;
				throw;
			}
			catch (RecognitionException e)
			{
				// we do not actually propogate ANTLRExceptions as a cause, so
				// log it here for diagnostic purposes
				if (log.IsInfoEnabled)
				{
					log.Info("converted antlr.RecognitionException", e);
				}

				throw QuerySyntaxException.Convert(e, _queryIdentifier);
			}

			_enabledFilters = null; //only needed during compilation phase...
		}

		private async Task<HqlSqlTranslator> AnalyzeAsync(string collectionRole)
		{
			var translator = new HqlSqlTranslator(_stageOneAst, this, _factory, _tokenReplacements, collectionRole);
			await (translator.TranslateAsync());
			return translator;
		}
	}

	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	internal partial class HqlSqlTranslator
	{
		public async Task<IStatement> TranslateAsync()
		{
			if (_resultAst == null)
			{
				if (_collectionRole != null)
				{
					HqlFilterPreprocessor.AddImpliedFromToQuery(_inputAst, _collectionRole, _sfi);
				}

				var nodes = new BufferedTreeNodeStream(_inputAst);
				var hqlSqlWalker = new HqlSqlWalker(_qti, _sfi, nodes, _tokenReplacements, _collectionRole);
				hqlSqlWalker.TreeAdaptor = new HqlSqlWalkerTreeAdaptor(hqlSqlWalker);
				try
				{
					// Transform the tree.
					_resultAst = (IStatement)(await (hqlSqlWalker.statementAsync())).Tree;
				}
				finally
				{
					hqlSqlWalker.ParseErrorHandler.ThrowQueryException();
				}
			}

			return _resultAst;
		}
	}
}
#endif
