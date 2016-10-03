#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateFixtureAsync : FixtureBaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.Dates.Mappings.Date.hbm.xml"};
			}
		}

		[Test]
		public async Task SavingAndRetrievingTestAsync()
		{
			DateTime Now = DateTime.Now;
			await (SavingAndRetrievingActionAsync(new AllDates{Sql_date = Now}, entity => DateTimeAssert.AreEqual(entity.Sql_date, Now, true)));
		}
	}
}
#endif
