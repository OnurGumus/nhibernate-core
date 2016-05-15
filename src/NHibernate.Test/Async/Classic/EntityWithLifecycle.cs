#if NET_4_5
using System.Collections.Generic;
using NHibernate.Classic;
using System.Threading.Tasks;
using System;
using NHibernate.Util;

namespace NHibernate.Test.Classic
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EntityWithLifecycle : ILifecycle
	{
		public virtual Task<LifecycleVeto> OnSaveAsync(ISession s)
		{
			try
			{
				return Task.FromResult<LifecycleVeto>(OnSave(s));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<LifecycleVeto>(ex);
			}
		}

		public virtual Task<LifecycleVeto> OnDeleteAsync(ISession s)
		{
			try
			{
				return Task.FromResult<LifecycleVeto>(OnDelete(s));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<LifecycleVeto>(ex);
			}
		}
	}
}
#endif
