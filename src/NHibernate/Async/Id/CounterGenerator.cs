#if NET_4_5
using System;
using System.Runtime.CompilerServices;
using NHibernate.Engine;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that returns a <c>Int64</c> constructed from the system
	/// time and a counter value. Not safe for use in a clustser!
	/// </summary>
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
