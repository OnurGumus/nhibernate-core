#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Test.Interceptor
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class StatefulInterceptor : EmptyInterceptor
	{
		public override async Task PostFlushAsync(ICollection entities)
		{
			if (list.Count > 0)
			{
				foreach (Log iter in list)
				{
					await (session.PersistAsync(iter));
				}

				list.Clear();
				await (session.FlushAsync());
			}
		}
	}
}
#endif
