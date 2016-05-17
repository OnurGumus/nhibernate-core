#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class UtcDateTimeTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			DateTime val = DateTime.UtcNow;
			DateTime expected = new DateTime(val.Year, val.Month, val.Day, val.Hour, val.Minute, val.Second, DateTimeKind.Utc);
			DateTimeClass basic = new DateTimeClass();
			basic.Id = 1;
			basic.UtcDateTimeValue = val;
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = (DateTimeClass)s.Load(typeof (DateTimeClass), 1);
			Assert.AreEqual(DateTimeKind.Utc, basic.UtcDateTimeValue.Value.Kind);
			Assert.AreEqual(expected, basic.UtcDateTimeValue.Value);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
