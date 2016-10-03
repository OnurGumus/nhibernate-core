#if NET_4_5
using System;
using System.Collections;
using System.Data;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTime2FixtureAsync : FixtureBaseAsync
	{
		protected override IList Mappings
		{
			get
			{
				return new[]{"NHSpecificTest.Dates.Mappings.DateTime2.hbm.xml"};
			}
		}

		protected override DbType? AppliesTo()
		{
			return DbType.DateTime2;
		}

		[Test]
		public async Task SavingAndRetrievingTestAsync()
		{
			DateTime Now = DateTime.Now;
			await (SavingAndRetrievingActionAsync(new AllDates{Sql_datetime2 = Now}, entity => DateTimeAssert.AreEqual(entity.Sql_datetime2, Now)));
			await (SavingAndRetrievingActionAsync(new AllDates{Sql_datetime2 = DateTime.MinValue}, entity => DateTimeAssert.AreEqual(entity.Sql_datetime2, DateTime.MinValue)));
			await (SavingAndRetrievingActionAsync(new AllDates{Sql_datetime2 = DateTime.MaxValue}, entity => DateTimeAssert.AreEqual(entity.Sql_datetime2, DateTime.MaxValue)));
		}

		[Test]
		public async Task SaveMillisecondAsync()
		{
			DateTime datetime2 = DateTime.MinValue.AddMilliseconds(123);
			await (SavingAndRetrievingActionAsync(new AllDates{Sql_datetime2 = datetime2}, entity => Assert.That(entity.Sql_datetime2, Is.EqualTo(datetime2))));
		}
	}
}
#endif
