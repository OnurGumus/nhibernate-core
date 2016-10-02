#if NET_4_5
using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Id.Insert;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class IdentityGenerator : AbstractPostInsertGenerator
	{
		/// <summary> 
		/// Delegate for dealing with IDENTITY columns where the dialect supports returning
		/// the generated IDENTITY value directly from the insert statement.
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class InsertSelectDelegate : AbstractReturningDelegate, IInsertGeneratedIdentifierDelegate
		{
			protected internal override Task<DbCommand> PrepareAsync(SqlCommandInfo insertSQL, ISessionImplementor session)
			{
				return session.Batcher.PrepareCommandAsync(CommandType.Text, insertSQL.Text, insertSQL.ParameterTypes);
			}

			public override async Task<object> ExecuteAndExtractAsync(DbCommand insert, ISessionImplementor session)
			{
				DbDataReader rs = await (session.Batcher.ExecuteReaderAsync(insert));
				try
				{
					return await (IdentifierGeneratorFactory.GetGeneratedIdentityAsync(rs, persister.IdentifierType, session));
				}
				finally
				{
					session.Batcher.CloseReader(rs);
				}
			}
		}

		/// <summary> 
		/// Delegate for dealing with IDENTITY columns where the dialect requires an
		/// additional command execution to retrieve the generated IDENTITY value
		/// </summary>
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class BasicDelegate : AbstractSelectingDelegate, IInsertGeneratedIdentifierDelegate
		{
			protected internal override Task<object> GetResultAsync(ISessionImplementor session, DbDataReader rs, object obj)
			{
				return IdentifierGeneratorFactory.GetGeneratedIdentityAsync(rs, persister.IdentifierType, session);
			}
		}
	}
}
#endif
