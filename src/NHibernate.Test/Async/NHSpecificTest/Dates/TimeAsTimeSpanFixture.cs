#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeAsTimeSpanFixtureAsync : FixtureBaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.Dates.Mappings.TimeAsTimeSpan.hbm.xml"};
			}
		}

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
