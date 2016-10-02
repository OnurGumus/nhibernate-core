#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Id;
using NHibernate.Intercept;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Tuple.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractEntityTuplizer : IEntityTuplizer
	{
		public virtual async Task<object[]> GetPropertyValuesToInsertAsync(object entity, IDictionary mergeMap, ISessionImplementor session)
		{
			int span = entityMetamodel.PropertySpan;
			object[] result = new object[span];
			for (int j = 0; j < span; j++)
			{
				result[j] = await (getters[j].GetForInsertAsync(entity, mergeMap, session));
			}

			return result;
		}
	}
}
#endif
