#if NET_4_5
using System.Collections.Generic;
using System.Data.Common;
using NHibernate.Engine;
using NHibernate.Id.Insert;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.SqlTypes;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary> 
	/// A generator that selects the just inserted row to determine the identifier
	/// value assigned by the database. The correct row is located using a unique key.
	/// </summary>
	/// <remarks>One mapping parameter is required: key (unless a natural-id is defined in the mapping).</remarks>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SelectGenerator : AbstractPostInsertGenerator, IConfigurable
	{
		/// <summary> The delegate for the select generation strategy.</summary>
		[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
		public partial class SelectGeneratorDelegate : AbstractSelectingDelegate
		{
			protected internal override async Task BindParametersAsync(ISessionImplementor session, DbCommand ps, object entity)
			{
				object uniqueKeyValue = ((IEntityPersister)persister).GetPropertyValue(entity, uniqueKeyPropertyName, session.EntityMode);
				await (uniqueKeyType.NullSafeSetAsync(ps, uniqueKeyValue, 0, session));
			}

			protected internal override async Task<object> GetResultAsync(ISessionImplementor session, DbDataReader rs, object entity)
			{
				if (!await (rs.ReadAsync()))
				{
					throw new IdentifierGenerationException("the inserted row could not be located by the unique key: " + uniqueKeyPropertyName);
				}

				return await (idType.NullSafeGetAsync(rs, persister.RootTableKeyColumnNames, session, entity));
			}
		}
	}
}
#endif
