using System;
using System.Runtime.CompilerServices;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref = "IIdentifierGenerator"/> that returns a <c>Int64</c> constructed from the system
	/// time and a counter value. Not safe for use in a clustser!
	/// </summary>
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CounterGenerator : IIdentifierGenerator
	{
		public async Task<object> GenerateAsync(ISessionImplementor cache, object obj)
		{
			return unchecked ((DateTime.Now.Ticks << 16) + Count);
		}
	}
}