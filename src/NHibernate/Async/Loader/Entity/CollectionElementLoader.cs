using System.Collections;
using System.Collections.Generic;
using System.Data;
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

namespace NHibernate.Loader.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CollectionElementLoader : OuterJoinLoader
	{
		protected override async Task<object> GetResultColumnOrRowAsync(object[] row, IResultTransformer transformer, IDataReader rs, ISessionImplementor session)
		{
			return row[row.Length - 1];
		}

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
	}
}