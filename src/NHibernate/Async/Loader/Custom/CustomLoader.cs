using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using NHibernate.Engine;
using NHibernate.Hql;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using IQueryable = NHibernate.Persister.Entity.IQueryable;
using System.Threading.Tasks;

namespace NHibernate.Loader.Custom
{
	/// <summary> 
	/// Extension point for loaders which use a SQL result set with "unexpected" column aliases. 
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomLoader : Loader
	{
		public Task<IList> ListAsync(ISessionImplementor session, QueryParameters queryParameters)
		{
			return ListAsync(session, queryParameters, querySpaces, resultTypes);
		}

		// Not ported: scroll
		protected override Task<object> GetResultColumnOrRowAsync(object[] row, IResultTransformer resultTransformer, IDataReader rs, ISessionImplementor session)
		{
			return rowProcessor.BuildResultRowAsync(row, rs, resultTransformer != null, session);
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ResultRowProcessor
		{
			/// <summary> Build a logical result row. </summary>
			/// <param name = "data">
			/// Entity data defined as "root returns" and already handled by the normal Loader mechanism.
			/// </param>
			/// <param name = "resultSet">The ADO result set (positioned at the row currently being processed). </param>
			/// <param name = "hasTransformer">Does this query have an associated <see cref = "IResultTransformer"/>. </param>
			/// <param name = "session">The session from which the query request originated.</param>
			/// <returns> The logical result row </returns>
			/// <remarks>
			/// At this point, Loader has already processed all non-scalar result data.  We
			/// just need to account for scalar result data here...
			/// </remarks>
			public async Task<object> BuildResultRowAsync(object[] data, IDataReader resultSet, bool hasTransformer, ISessionImplementor session)
			{
				object[] resultRow;
				// NH Different behavior (patched in NH-1612 to solve Hibernate issue HHH-2831).
				if (!hasScalars && (hasTransformer || data.Length == 0))
				{
					resultRow = data;
				}
				else
				{
					// build an array with indices equal to the total number
					// of actual returns in the result Hibernate will return
					// for this query (scalars + non-scalars)
					resultRow = new object[columnProcessors.Length];
					for (int i = 0; i < columnProcessors.Length; i++)
					{
						resultRow[i] = await (columnProcessors[i].ExtractAsync(data, resultSet, session));
					}
				}

				return (hasTransformer) ? resultRow : (resultRow.Length == 1) ? resultRow[0] : resultRow;
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial interface IResultColumnProcessor
		{
			Task<object> ExtractAsync(object[] data, IDataReader resultSet, ISessionImplementor session);
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class NonScalarResultColumnProcessor : IResultColumnProcessor
		{
			public Task<object> ExtractAsync(object[] data, IDataReader resultSet, ISessionImplementor session)
			{
				try
				{
					return Task.FromResult<object>(Extract(data, resultSet, session));
				}
				catch (Exception ex)
				{
					return TaskHelper.FromException<object>(ex);
				}
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ScalarResultColumnProcessor : IResultColumnProcessor
		{
			public Task<object> ExtractAsync(object[] data, IDataReader resultSet, ISessionImplementor session)
			{
				return type.NullSafeGetAsync(resultSet, alias, session, null);
			}
		}
	}
}