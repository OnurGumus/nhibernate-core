#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Int16TypeFixture
	{
		[Test]
		public async Task NextAsync()
		{
			Int16Type type = (Int16Type)NHibernateUtil.Int16;
			object current = (short)1;
			object next = await (type.NextAsync(current, null));
			Assert.IsTrue(next is Int16, "Next should be Int16");
			Assert.AreEqual((short)2, (short)next, "current should have been incremented to 2");
		}

		[Test]
		public async Task SeedAsync()
		{
			Int16Type type = (Int16Type)NHibernateUtil.Int16;
			Assert.IsTrue(await (type.SeedAsync(null)) is Int16, "seed should be int16");
		}
	}
}
#endif
