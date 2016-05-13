using System.Collections.Generic;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;
using IQueryable = NHibernate.Persister.Entity.IQueryable;
using System.Threading.Tasks;
using System;

namespace NHibernate.Loader.Criteria
{
	/// <summary>
	/// A <see cref = "JoinWalker"/> for <see cref = "ICriteria"/> queries.
	/// </summary>
	/// <seealso cref = "CriteriaLoader"/>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CriteriaJoinWalker : AbstractEntityJoinWalker
	{
		protected override async Task WalkEntityTreeAsync(IOuterJoinLoadable persister, string alias, string path, int currentDepth)
		{
			// NH different behavior (NH-1476, NH-1760, NH-1785)
			await (base.WalkEntityTreeAsync(persister, alias, path, currentDepth));
			await (WalkCompositeComponentIdTreeAsync(persister, alias, path));
		}

		private async Task WalkCompositeComponentIdTreeAsync(IOuterJoinLoadable persister, string alias, string path)
		{
			IType type = persister.IdentifierType;
			string propertyName = persister.IdentifierPropertyName;
			if (type != null && type.IsComponentType)
			{
				ILhsAssociationTypeSqlInfo associationTypeSQLInfo = JoinHelper.GetIdLhsSqlInfo(alias, persister, Factory);
				await (WalkComponentTreeAsync((IAbstractComponentType)type, 0, alias, SubPath(path, propertyName), 0, associationTypeSQLInfo));
			}
		}

		protected override Task<SqlString> GetWithClauseAsync(string path)
		{
			return translator.GetWithClauseAsync(path, EnabledFilters);
		}
	}
}