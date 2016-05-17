#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeSpanFixture2 : TypeFixtureBase
	{
		[Test]
		public async Task SavingAndRetrievingAsync()
		{
			var ticks = DateTime.Parse("23:59:59").TimeOfDay;
			var entity = new TimeAsTimeSpanClass{TimeSpanValue = ticks};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(entity));
					await (tx.CommitAsync());
				}

			TimeAsTimeSpanClass entityReturned;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					entityReturned = s.CreateQuery("from TimeAsTimeSpanClass").UniqueResult<TimeAsTimeSpanClass>();
					Assert.AreEqual(ticks, entityReturned.TimeSpanValue);
					Assert.AreEqual(entityReturned.TimeSpanValue.Hours, ticks.Hours);
					Assert.AreEqual(entityReturned.TimeSpanValue.Minutes, ticks.Minutes);
					Assert.AreEqual(entityReturned.TimeSpanValue.Seconds, ticks.Seconds);
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync(entityReturned));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
