#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.Linq;
using NHibernate.Driver;
using NHibernate.Type;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeOffsetQueryFixture : FixtureBase
	{
		[Test]
		public async Task CanQueryWithCastInHqlAsync()
		{
			using (ISession s = OpenSession())
				using (s.BeginTransaction())
				{
					var datesRecovered = await (s.CreateQuery("select cast(min(Sql_datetimeoffset), datetimeoffset) from AllDates").UniqueResultAsync<DateTimeOffset>());
					Assert.That(datesRecovered, Is.EqualTo(new DateTimeOffset(2012, 11, 1, 2, 0, 0, TimeSpan.FromHours(3))));
				}
		}
	}
}
#endif
