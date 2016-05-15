#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Int32TypeFixture
	{
		[Test]
		public async Task NextAsync()
		{
			Int32Type type = (Int32Type)NHibernateUtil.Int32;
			object current = (int)1;
			object next = await (type.NextAsync(current, null));
			Assert.IsTrue(next is Int32, "Next should be Int32");
			Assert.AreEqual((int)2, (int)next, "current should have been incremented to 2");
		}

		[Test]
		public async Task SeedAsync()
		{
			Int32Type type = (Int32Type)NHibernateUtil.Int32;
			Assert.IsTrue(await (type.SeedAsync(null)) is Int32, "seed should be Int32");
		}
	}
}
#endif
