#if NET_4_5
using System.Collections.Generic;
using NHibernate.Util;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.UtilityTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SafetyEnumerableFixtureAsync
	{
		[Test]
		public void MixedCollection()
		{
			IList<IAType> l = new List<IAType>();
			l.Add(new AClass());
			l.Add(new BClass());
			l.Add(new AClass());
			l.Add(new BClass());
			l.Add(new AClass());
			IEnumerable<AClass> eAClass = new SafetyEnumerable<AClass>(l);
			int i = 0;
			foreach (IAType aClass in eAClass)
				++i;
			Assert.AreEqual(3, i);
			IEnumerable<BClass> eBClass = new SafetyEnumerable<BClass>(l);
			i = 0;
			foreach (IAType aClass in eBClass)
				++i;
			Assert.AreEqual(2, i);
		}

		[Test]
		public void RecyclingCollection()
		{
			IList<IAType> l = new List<IAType>();
			l.Add(new AClass());
			l.Add(new BClass());
			l.Add(new AClass());
			l.Add(new BClass());
			l.Add(new AClass());
			IEnumerable<AClass> eAClass = new SafetyEnumerable<AClass>(l);
			int i = 0;
			foreach (IAType aClass in eAClass)
				++i;
			Assert.AreEqual(3, i);
			i = 0;
			foreach (IAType aClass in eAClass)
				++i;
			Assert.AreEqual(3, i);
		}

		[Test]
		public void MixedWithNulls()
		{
			IList<IAType> l = new List<IAType>();
			l.Add(new AClass());
			l.Add(null);
			l.Add(new AClass());
			l.Add(new BClass());
			l.Add(new AClass());
			IEnumerable<AClass> eAClass = new SafetyEnumerable<AClass>(l);
			int i = 0;
			foreach (IAType aClass in eAClass)
				++i;
			Assert.AreEqual(4, i);
			IEnumerable<BClass> eBClass = new SafetyEnumerable<BClass>(l);
			i = 0;
			foreach (IAType aClass in eBClass)
				++i;
			Assert.AreEqual(2, i);
		}
	}
}
#endif
