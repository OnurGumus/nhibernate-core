﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Data;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Id.Insert;
using NHibernate.SqlCommand;

namespace NHibernate.Id
{
	using System.Threading.Tasks;
	/// <content>
	/// Contains generated async methods
	/// </content>
	public partial class IdentityGenerator : AbstractPostInsertGenerator
	{

		/// <content>
		/// Contains generated async methods
		/// </content>
		public partial class InsertSelectDelegate : AbstractReturningDelegate, IInsertGeneratedIdentifierDelegate
		{

			protected internal override Task<DbCommand> PrepareAsync(SqlCommandInfo insertSQL, ISessionImplementor session)
			{
				return session.Batcher.PrepareCommandAsync(CommandType.Text, insertSQL.Text, insertSQL.ParameterTypes);
			}

			public override async Task<object> ExecuteAndExtractAsync(DbCommand insert, ISessionImplementor session)
			{
				var rs = await (session.Batcher.ExecuteReaderAsync(insert)).ConfigureAwait(false);
				try
				{
					return await (IdentifierGeneratorFactory.GetGeneratedIdentityAsync(rs, persister.IdentifierType, session)).ConfigureAwait(false);
				}
				finally
				{
					session.Batcher.CloseReader(rs);
				}
			}
		}

		/// <content>
		/// Contains generated async methods
		/// </content>
		public partial class BasicDelegate : AbstractSelectingDelegate, IInsertGeneratedIdentifierDelegate
		{

			protected internal override Task<object> GetResultAsync(ISessionImplementor session, DbDataReader rs, object obj)
			{
				return IdentifierGeneratorFactory.GetGeneratedIdentityAsync(rs, persister.IdentifierType, session);
			}
		}
	}
}