#if NET_4_5
using System;
using NHibernate.Cache;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.CacheTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimestamperFixtureAsync
	{
		[Test, Explicit]
		public void VerifyIncrease()
		{
			long currentTicks = 0;
			long newTicks = 0;
			// the Timestampper will only generate 4095 increasing identifiers per millisecond.
			for (int i = 0; i < 4095; i++)
			{
				newTicks = Timestamper.Next();
				if ((newTicks - currentTicks) == 0)
				{
					Assert.Fail("diff was " + (newTicks - currentTicks) + ".  It should always increase.  Loop i=" + i + " with currentTicks = " + currentTicks + " and newTicks = " + newTicks);
				}

				currentTicks = newTicks;
			}
		}
	}
}
#endif
