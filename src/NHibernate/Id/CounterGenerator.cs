using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NHibernate.Engine;

namespace NHibernate.Id
{
	/// <summary>
	/// An <see cref="IIdentifierGenerator" /> that returns a <c>Int64</c> constructed from the system
	/// time and a counter value. Not safe for use in a clustser!
	/// </summary>
	public class CounterGenerator : IIdentifierGenerator
	{
		// (short)0 by default
		private static short counter;

		protected short Count
		{
			[MethodImpl(MethodImplOptions.Synchronized)]
			get
			{
				if (counter < 0)
				{
					counter = 0;
				}
				return counter++;
			}
		}

		public Task<object> Generate(ISessionImplementor cache, object obj)
		{
			return Task.FromResult<object>(unchecked ((DateTime.Now.Ticks << 16) + Count));
		}
	}
}