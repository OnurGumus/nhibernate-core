#if NET_4_5
using System;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Classic;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1914
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomList<T> : IList<T>, IList, ILifecycle
	{
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
	}
}
#endif
