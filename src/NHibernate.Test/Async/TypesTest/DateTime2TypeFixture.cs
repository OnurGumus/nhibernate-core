#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTime2TypeFixture
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

		[Test]
		public async Task DeepCopyNotNullAsync()
		{
			NullableType type = NHibernateUtil.DateTime;
			object value1 = DateTime.Now;
			object value2 = await (type.DeepCopyAsync(value1, EntityMode.Poco, null));
			Assert.AreEqual(value1, value2, "Copies should be the same.");
			value2 = ((DateTime)value2).AddHours(2);
			Assert.IsFalse(value1 == value2, "value2 was changed, value1 should not have changed also.");
		}

		[Test]
		public async Task EqualityShouldIgnoreKindAndNotIgnoreMillisecondAsync()
		{
			var type = NHibernateUtil.DateTime;
			var localTime = DateTime.Now;
			var unspecifiedKid = new DateTime(localTime.Ticks, DateTimeKind.Unspecified);
			Assert.That(type.IsEqual(localTime, unspecifiedKid), Is.True);
			Assert.That(await (type.IsEqualAsync(localTime, unspecifiedKid, EntityMode.Poco)), Is.True);
		}
	}
}
#endif
