#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using NHibernate.Dialect.Function;
using NHibernate.Dialect.Lock;
using NHibernate.Dialect.Schema;
using NHibernate.Exceptions;
using NHibernate.Id;
using NHibernate.Mapping;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using NHibernate.Util;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Dialect
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class Dialect
	{
		/// <summary> 
		/// Given a callable statement previously processed by <see cref = "RegisterResultSetOutParameter"/>,
		/// extract the <see cref = "DbDataReader"/> from the OUT parameter. 
		/// </summary>
		/// <param name = "statement">The callable statement. </param>
		/// <returns> The extracted result set. </returns>
		/// <throws>  SQLException Indicates problems extracting the result set. </throws>
		public virtual Task<DbDataReader> GetResultSetAsync(DbCommand statement)
		{
			try
			{
				return Task.FromResult<DbDataReader>(GetResultSet(statement));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<DbDataReader>(ex);
			}
		}
	}
}
#endif
