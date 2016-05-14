#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR.Tree;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Hql.Ast.ANTLR
{
	/// <summary>
	/// Generates translators which uses the Antlr-based parser to perform
	/// the translation.
	/// 
	/// Author: Gavin King
	/// Ported by: Steve Strong
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ASTQueryTranslatorFactory : IQueryTranslatorFactory
	{
		public async Task<IQueryTranslator[]> CreateQueryTranslatorsAsync(string queryString, string collectionRole, bool shallow, IDictionary<string, IFilter> filters, ISessionFactoryImplementor factory)
		{
			return await (CreateQueryTranslatorsAsync(queryString.ToQueryExpression(), collectionRole, shallow, filters, factory));
		}

		public Task<IQueryTranslator[]> CreateQueryTranslatorsAsync(IQueryExpression queryExpression, string collectionRole, bool shallow, IDictionary<string, IFilter> filters, ISessionFactoryImplementor factory)
		{
			try
			{
				return Task.FromResult<IQueryTranslator[]>(CreateQueryTranslators(queryExpression, collectionRole, shallow, filters, factory));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<IQueryTranslator[]>(ex);
			}
		}

		static async Task<IQueryTranslator[]> CreateQueryTranslatorsAsync(IASTNode ast, string queryIdentifier, string collectionRole, bool shallow, IDictionary<string, IFilter> filters, ISessionFactoryImplementor factory)
		{
			var polymorphicParsers = AstPolymorphicProcessor.Process(ast, factory);
			var translators = polymorphicParsers.Select(hql => new QueryTranslatorImpl(queryIdentifier, hql, filters, factory)).ToArray();
			foreach (var translator in translators)
			{
				if (collectionRole == null)
				{
					await (translator.CompileAsync(factory.Settings.QuerySubstitutions, shallow));
				}
				else
				{
					await (translator.CompileAsync(collectionRole, factory.Settings.QuerySubstitutions, shallow));
				}
			}

			return translators;
		}
	}
}
#endif
