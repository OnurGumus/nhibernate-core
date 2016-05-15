#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate.Type;
using NUnit.Framework;
using NHibernate.Engine;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Int64TypeFixture : TestCase
	{
		[Test]
		public async Task NextAsync()
		{
			Int64Type type = (Int64Type)NHibernateUtil.Int64;
			object current = (long)1;
			object next = await (type.NextAsync(current, null));
			Assert.IsTrue(next is Int64, "Next should be Int64");
			Assert.AreEqual((long)2, (long)next, "current should have been incremented to 2");
		}

		[Test]
		public async Task SeedAsync()
		{
			Int64Type type = (Int64Type)NHibernateUtil.Int64;
			Assert.IsTrue(await (type.SeedAsync(null)) is Int64, "seed should be int64");
		}

		[Test]
		public async Task NullableWrapperDirtyAsync()
		{
			Int64Type type = (Int64Type)NHibernateUtil.Int64;
			long ? nullLong = null;
			long ? valueLong = 5;
			long ? fiveAgain = 5;
			using (ISession s = OpenSession())
			{
				Assert.IsTrue(await (type.IsDirtyAsync(nullLong, valueLong, (ISessionImplementor)s)), "should be dirty - null to '5'");
				Assert.IsFalse(await (type.IsDirtyAsync(valueLong, fiveAgain, (ISessionImplementor)s)), "should not be dirty - 5 to 5");
			}
		}
	}
}
#endif
