#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TicksTypeFixture
	{
		[Test]
		public async Task NextAsync()
		{
			TicksType type = (TicksType)NHibernateUtil.Ticks;
			object current = new DateTime(2004, 1, 1, 1, 1, 1, 1);
			object next = await (type.NextAsync(current, null));
			Assert.IsTrue(next is DateTime, "Next should be DateTime");
			Assert.IsTrue((DateTime)next > (DateTime)current, "next should be greater than current (could be equal depending on how quickly this occurs)");
		}

		[Test]
		public async Task SeedAsync()
		{
			TicksType type = (TicksType)NHibernateUtil.Ticks;
			Assert.IsTrue(await (type.SeedAsync(null)) is DateTime, "seed should be DateTime");
		}

		[Test]
		public async Task ComparerAsync()
		{
			var type = (IVersionType)NHibernateUtil.Ticks;
			object v1 = await (type.SeedAsync(null));
			var v2 = v1;
			Assert.DoesNotThrow(() => type.Comparator.Compare(v1, v2));
		}
	}
}
#endif
