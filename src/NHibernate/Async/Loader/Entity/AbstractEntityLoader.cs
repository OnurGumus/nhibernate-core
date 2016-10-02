#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Loader.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEntityLoader : OuterJoinLoader, IUniqueEntityLoader
	{
		public Task<object> LoadAsync(object id, object optionalObject, ISessionImplementor session)
		{
			return LoadAsync(session, id, optionalObject, id);
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

		protected override Task<object> GetResultColumnOrRowAsync(object[] row, IResultTransformer resultTransformer, DbDataReader rs, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(GetResultColumnOrRow(row, resultTransformer, rs, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
