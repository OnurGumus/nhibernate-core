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
		public async Task<IList> ListAsync(ISessionImplementor session, QueryParameters queryParameters)
		{
			return await (ListAsync(session, queryParameters, querySpaces, resultTypes));
		}

		protected override async Task<object> GetResultColumnOrRowAsync(object[] row, IResultTransformer resultTransformer, IDataReader rs, ISessionImplementor session)
		{
			return await (rowProcessor.BuildResultRowAsync(row, rs, resultTransformer != null, session));
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ScalarResultColumnProcessor : IResultColumnProcessor
		{
			public async Task<object> ExtractAsync(object[] data, IDataReader resultSet, ISessionImplementor session)
			{
				return await (type.NullSafeGetAsync(resultSet, alias, session, null));
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class NonScalarResultColumnProcessor : IResultColumnProcessor
		{
			public async Task<object> ExtractAsync(object[] data, IDataReader resultSet, ISessionImplementor session)
			{
				return data[position];
			}
		}

		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class ResultRowProcessor
		{
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
	}
}