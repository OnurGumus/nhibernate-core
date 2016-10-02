#if NET_4_5
using System;
using System.Collections.Generic;
using System.Reflection;
using NHibernate.Bytecode;
using NHibernate.Classic;
using NHibernate.Engine;
using NHibernate.Intercept;
using NHibernate.Mapping;
using NHibernate.Properties;
using NHibernate.Proxy;
using NHibernate.Type;
using NHibernate.Util;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NHibernate.Tuple.Entity
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PocoEntityTuplizer : AbstractEntityTuplizer
	{
		public override async Task<object[]> GetPropertyValuesToInsertAsync(object entity, System.Collections.IDictionary mergeMap, ISessionImplementor session)
		{
			if (ShouldGetAllProperties(entity) && optimizer != null && optimizer.AccessOptimizer != null)
			{
				return GetPropertyValuesWithOptimizer(entity);
			}
			else
			{
				return await (base.GetPropertyValuesToInsertAsync(entity, mergeMap, session));
			}
		}
	}
}
#endif
