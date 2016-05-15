#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeTypeFixture
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
		public async Task EqualityShouldIgnoreKindAndMillisecondAsync()
		{
			var type = (DateTimeType)NHibernateUtil.DateTime;
			var localTime = DateTime.Now;
			var unspecifiedKid = new DateTime(localTime.Year, localTime.Month, localTime.Day, localTime.Hour, localTime.Minute, localTime.Second, 0, DateTimeKind.Unspecified);
			Assert.That(type.IsEqual(localTime, unspecifiedKid), Is.True);
			Assert.That(await (type.IsEqualAsync(localTime, unspecifiedKid, EntityMode.Poco)), Is.True);
		}
	}
}
#endif
