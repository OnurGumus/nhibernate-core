#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NHibernate.Type;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using NHibernate.Util;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTypeFixtureAsync : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "Date";
			}
		}

		[Test]
		public async Task ReadWriteNormalAsync()
		{
			var expected = DateTime.Today.Date;
			var basic = new DateClass{DateValue = expected.AddHours(1)};
			object savedId;
			using (ISession s = OpenSession())
			{
				savedId = await (s.SaveAsync(basic));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				basic = await (s.GetAsync<DateClass>(savedId));
				Assert.That(basic.DateValue, Is.EqualTo(expected));
				await (s.DeleteAsync(basic));
				await (s.FlushAsync());
			}
		}

		[Test]
		public async Task ReadWriteBaseValueAsync()
		{
			var basic = new DateClass{DateValue = new DateTime(1899, 1, 1)};
			object savedId;
			using (ISession s = OpenSession())
			{
				savedId = await (s.SaveAsync(basic));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				basic = await (s.GetAsync<DateClass>(savedId));
				Assert.That(basic.DateValue.HasValue, Is.False);
				await (s.DeleteAsync(basic));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
