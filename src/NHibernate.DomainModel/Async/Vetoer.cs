#if NET_4_5
using System;
using NHibernate.Classic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.DomainModel
{
	/// <summary>
	/// Summary description for Vetoer.
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Vetoer : ILifecycle
	{
		public Task<LifecycleVeto> OnSaveAsync(ISession s)
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

		public Task<LifecycleVeto> OnDeleteAsync(ISession s)
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
