#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Param;
using NHibernate.Persister.Collection;
using NHibernate.Persister.Entity;
using NHibernate.SqlCommand;
using NHibernate.Transform;
using NHibernate.Type;
using NHibernate.Util;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace NHibernate.Loader.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionElementLoader : OuterJoinLoader
	{
		public virtual async Task<object> LoadElementAsync(ISessionImplementor session, object key, object index)
		{
			IList list = await (LoadEntityAsync(session, key, index, keyType, indexType, persister));
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
					throw new HibernateException("More than one row was found");
				}
			}
		}

		protected override Task<object> GetResultColumnOrRowAsync(object[] row, IResultTransformer transformer, DbDataReader rs, ISessionImplementor session)
		{
			try
			{
				return Task.FromResult<object>(GetResultColumnOrRow(row, transformer, rs, session));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
