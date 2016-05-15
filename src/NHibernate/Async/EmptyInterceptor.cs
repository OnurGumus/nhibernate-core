#if NET_4_5
using System;
using System.Collections;
using NHibernate.SqlCommand;
using NHibernate.Type;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EmptyInterceptor : IInterceptor
	{
		public virtual Task PostFlushAsync(ICollection entities)
		{
			return TaskHelper.CompletedTask;
		}
	}
}

namespace NHibernate
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EmptyInterceptor : IInterceptor
	{
		public virtual Task PostFlushAsync(ICollection entities)
		{
			return TaskHelper.CompletedTask;
		}
	}
}
#endif
