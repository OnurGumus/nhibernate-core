#if NET_4_5
using System;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LocalDateTimeTypeFixture : TypeFixtureBase
	{
		[Test]
		public async Task ReadWriteAsync()
		{
			DateTime val = DateTime.UtcNow;
			DateTime expected = new DateTime(val.Year, val.Month, val.Day, val.Hour, val.Minute, val.Second, DateTimeKind.Local);
			DateTimeClass basic = new DateTimeClass();
			basic.Id = 1;
			basic.LocalDateTimeValue = val;
			ISession s = OpenSession();
			await (s.SaveAsync(basic));
			await (s.FlushAsync());
			s.Close();
			s = OpenSession();
			basic = (DateTimeClass)s.Load(typeof (DateTimeClass), 1);
			Assert.AreEqual(DateTimeKind.Local, basic.LocalDateTimeValue.Value.Kind);
			Assert.AreEqual(expected, basic.LocalDateTimeValue.Value);
			await (s.DeleteAsync(basic));
			await (s.FlushAsync());
			s.Close();
		}
	}
}
#endif
