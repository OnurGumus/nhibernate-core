#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Classic;
using System.Threading.Tasks;

namespace NHibernate.DomainModel
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fum : ILifecycle
	{
		public async Task<LifecycleVeto> OnDeleteAsync(ISession s)
		{
			if (_friends == null)
				return LifecycleVeto.NoVeto;
			try
			{
				foreach (object obj in _friends)
				{
					await (s.DeleteAsync(obj));
				}
			}
			catch (Exception e)
			{
				throw new CallbackException(e);
			}

			return LifecycleVeto.NoVeto;
		}

		public async Task<LifecycleVeto> OnSaveAsync(ISession s)
		{
			if (_friends == null)
				return LifecycleVeto.NoVeto;
			try
			{
				foreach (object obj in _friends)
				{
					await (s.SaveAsync(obj));
				}
			}
			catch (Exception e)
			{
				throw new CallbackException(e);
			}

			return LifecycleVeto.NoVeto;
		}
	}
}
#endif
