#if NET_4_5
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1608
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync
	{
		[Test]
		public void AddDoesBoundsChecking()
		{
			var map = new LRUMap(128);
			for (int i = 0; i < 200; i++)
				map.Add("str" + i, i);
			Assert.That(map.Count, Is.EqualTo(128));
		}

		[Test]
		public void IndexerDoesBoundsChecking()
		{
			var map = new LRUMap(128);
			for (int i = 0; i < 200; i++)
				map["str" + i] = i;
			Assert.That(map.Count, Is.EqualTo(128));
		}
	}
}
#endif
