#if NET_4_5
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using NHibernate.Driver;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Dates
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DateTimeOffsetFixture : FixtureBase
	{
		[Test]
		public async Task SavingAndRetrievingTestAsync()
		{
			DateTimeOffset NowOS = DateTimeOffset.Now;
			AllDates dates = new AllDates{Sql_datetimeoffset = NowOS};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(dates));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var datesRecovered = s.CreateQuery("from AllDates").UniqueResult<AllDates>();
					Assert.That(datesRecovered.Sql_datetimeoffset, Is.EqualTo(NowOS));
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					var datesRecovered = s.CreateQuery("from AllDates").UniqueResult<AllDates>();
					await (s.DeleteAsync(datesRecovered));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
