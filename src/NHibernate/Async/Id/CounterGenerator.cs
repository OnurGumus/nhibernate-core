#if NET_4_5
using System;
using System.Runtime.CompilerServices;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Id
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CounterGenerator : IIdentifierGenerator
	{
		public Task<object> GenerateAsync(ISessionImplementor cache, object obj)
		{
			try
			{
				return Task.FromResult<object>(Generate(cache, obj));
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}
	}
}
#endif
