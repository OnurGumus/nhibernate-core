#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeAsTimeSpanTypeFixtureAsync
	{
		[Test]
		public async Task NextAsync()
		{
			var type = (TimeAsTimeSpanType)NHibernateUtil.TimeAsTimeSpan;
			object current = new TimeSpan(DateTime.Now.Ticks - 5);
			object next = await (type.NextAsync(current, null));
			Assert.IsTrue(next is TimeSpan, "Next should be TimeSpan");
			Assert.IsTrue((TimeSpan)next > (TimeSpan)current, "next should be greater than current (could be equal depending on how quickly this occurs)");
		}

		[Test]
		public async Task SeedAsync()
		{
			var type = (TimeAsTimeSpanType)NHibernateUtil.TimeAsTimeSpan;
			Assert.IsTrue(await (type.SeedAsync(null)) is TimeSpan, "seed should be TimeSpan");
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeSpanFixture2Async : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "TimeAsTimeSpan";
			}
		}

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
					entityReturned = await (s.CreateQuery("from TimeAsTimeSpanClass").UniqueResultAsync<TimeAsTimeSpanClass>());
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
