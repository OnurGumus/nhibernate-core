using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Loader.Entity
{
	/// <summary>
	/// Abstract superclass for entity loaders that use outer joins
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEntityLoader : OuterJoinLoader, IUniqueEntityLoader
	{
		public async Task<object> LoadAsync(object id, object optionalObject, ISessionImplementor session)
		{
			return Load(session, id, optionalObject, id);
		}

		protected override async Task<object> GetResultColumnOrRowAsync(object[] row, IResultTransformer resultTransformer, IDataReader rs, ISessionImplementor session)
		{
			return row[row.Length - 1];
		}

		protected virtual async Task<object> LoadAsync(ISessionImplementor session, object id, object optionalObject, object optionalId)
		{
			IList list = await (LoadEntityAsync(session, id, UniqueKeyType, optionalObject, entityName, optionalId, persister));
			if (list.Count == 1)
			{
				return list[0];
			}
			else if (list.Count == 0)
			{
				return null;
			}
			else
			{
				if (CollectionOwners != null)
				{
					return list[0];
				}
				else
				{
					throw new HibernateException(string.Format("More than one row with the given identifier was found: {0}, for class: {1}", id, persister.EntityName));
				}
			}
		}
	}
}