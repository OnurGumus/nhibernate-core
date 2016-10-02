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
	public partial class DateTime2TypeFixtureAsync
	{
		[Test]
		public async Task NextAsync()
		{
			DateTimeType type = NHibernateUtil.DateTime2;
			object current = DateTime.Now.AddMilliseconds(-1);
			object next = await (type.NextAsync(current, null));
			Assert.That(next, Is.TypeOf<DateTime>().And.GreaterThan(current));
		}

		[Test]
		public async Task SeedAsync()
		{
			DateTimeType type = NHibernateUtil.DateTime;
			Assert.IsTrue(await (type.SeedAsync(null)) is DateTime, "seed should be DateTime");
		}
	}
}
#endif
