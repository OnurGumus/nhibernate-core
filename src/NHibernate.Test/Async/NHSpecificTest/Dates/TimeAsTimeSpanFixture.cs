#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeAsTimeSpanFixture : FixtureBase
	{
		[Test]
		public async Task SavingAndRetrievingTestAsync()
		{
			TimeSpan now = DateTime.Parse("23:59:59").TimeOfDay;
			await (SavingAndRetrievingActionAsync(new AllDates{Sql_TimeAsTimeSpan = now}, entity =>
			{
				Assert.AreEqual(entity.Sql_TimeAsTimeSpan.Hours, now.Hours);
				Assert.AreEqual(entity.Sql_TimeAsTimeSpan.Minutes, now.Minutes);
				Assert.AreEqual(entity.Sql_TimeAsTimeSpan.Seconds, now.Seconds);
			}

			));
		}
	}
}
#endif
