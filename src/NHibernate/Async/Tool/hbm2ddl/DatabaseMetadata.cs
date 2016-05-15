#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using NHibernate.Dialect.Schema;
using NHibernate.Exceptions;
using NHibernate.Mapping;
using NHibernate.Util;
using System.Threading.Tasks;

namespace NHibernate.Tool.hbm2ddl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DatabaseMetadata
	{
		private async Task InitSequencesAsync(DbConnection connection, Dialect.Dialect dialect)
		{
			if (dialect.SupportsSequences)
			{
				string sql = dialect.QuerySequencesString;
				if (sql != null)
				{
					using (DbCommand statement = connection.CreateCommand())
					{
						statement.CommandText = sql;
						using (DbDataReader rs = await (statement.ExecuteReaderAsync()))
						{
							while (await (rs.ReadAsync()))
								sequences.Add(((string)rs[0]).ToLower().Trim());
						}
					}
				}
			}
		}
	}
}
#endif
