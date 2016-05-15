#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeFixture : FixtureBase
	{
		[Test]
		public async Task SavingAndRetrievingTestAsync()
		{
			DateTime now = DateTime.Parse("23:59:59");
			await (SavingAndRetrievingActionAsync(new AllDates{Sql_time = now}, entity =>
			{
				Assert.AreEqual(entity.Sql_time.Hour, now.Hour);
				Assert.AreEqual(entity.Sql_time.Minute, now.Minute);
				Assert.AreEqual(entity.Sql_time.Second, now.Second);
			}

			));
		}
	}
}
#endif
