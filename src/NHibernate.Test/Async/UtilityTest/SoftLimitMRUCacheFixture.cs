#if NET_4_5
using System;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SoftLimitMRUCacheFixtureAsync
	{
		[Test]
		public void DontFillUp()
		{
			// NH-1671
			const int count = 32;
			var s = new SoftLimitMRUCache(count);
			for (int i = 0; i < count + 10; i++)
			{
				s.Put(new object (), new object ());
			}

			Assert.That(s.Count, Is.EqualTo(count));
			GC.Collect();
			s.Put(new object (), new object ());
			Assert.That(s.SoftCount, Is.EqualTo(count + 1));
		}
	}
}
#endif
