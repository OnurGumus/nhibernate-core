#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeTypeFixtureAsync
	{
		[Test]
		public async Task NextAsync()
		{
			DateTimeType type = (DateTimeType)NHibernateUtil.DateTime;
			object current = DateTime.Parse("2004-01-01");
			object next = await (type.NextAsync(current, null));
			Assert.IsTrue(next is DateTime, "Next should be DateTime");
			Assert.IsTrue((DateTime)next > (DateTime)current, "next should be greater than current (could be equal depending on how quickly this occurs)");
		}

		[Test]
		public async Task SeedAsync()
		{
			DateTimeType type = (DateTimeType)NHibernateUtil.DateTime;
			Assert.IsTrue(await (type.SeedAsync(null)) is DateTime, "seed should be DateTime");
		}
	}
}
#endif
