#if NET_4_5
using System;
using System.Data;
using NHibernate.Engine;
using NHibernate.Id.Insert;
using NHibernate.SqlCommand;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that indicates to the <see cref = "ISession"/> that identity
	/// (ie. identity/autoincrement column) key generation should be used.
	/// </summary>
	/// <remarks>
	/// <p>
	///	This id generation strategy is specified in the mapping file as 
	///	<code>&lt;generator class="identity" /&gt;</code> 
	///	or if the database natively supports identity columns 
	///	<code>&lt;generator class="native" /&gt;</code>
	/// </p>
	/// <p>
	/// This indicates to NHibernate that the database generates the id when
	/// the entity is inserted.
	/// </p>
	/// </remarks>
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
			public override async Task<object> ExecuteAndExtractAsync(IDbCommand insert, ISessionImplementor session)
			{
				IDataReader rs = await (session.Batcher.ExecuteReaderAsync(insert));
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
			protected internal override Task<object> GetResultAsync(ISessionImplementor session, IDataReader rs, object obj)
			{
				return IdentifierGeneratorFactory.GetGeneratedIdentityAsync(rs, persister.IdentifierType, session);
			}
		}
	}
}
#endif
