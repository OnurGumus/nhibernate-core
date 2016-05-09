using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Engine;
using NHibernate.Proxy;
using NHibernate.Transform;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Impl
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractDetachedQuery : IDetachedQuery, IDetachedQueryImplementor
	{
		public async Task<IDetachedQuery> SetEntityAsync(string name, object val)
		{
			SetParameter(name, val, NHibernateUtil.Entity(await (NHibernateProxyHelper.GuessClassAsync(val))));
			return this;
		}

		public async Task<IDetachedQuery> SetEntityAsync(int position, object val)
		{
			SetParameter(position, val, NHibernateUtil.Entity(await (NHibernateProxyHelper.GuessClassAsync(val))));
			return this;
		}
	}
}