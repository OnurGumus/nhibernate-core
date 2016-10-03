#if NET_4_5
using System;
using NHibernate.Type;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.TypesTest
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeSpanTypeFixtureAsync
	{
		[Test]
		public async Task NextAsync()
		{
			var type = (TimeSpanType)NHibernateUtil.TimeSpan;
			object current = new TimeSpan(DateTime.Now.Ticks - 5);
			object next = await (type.NextAsync(current, null));
			Assert.IsTrue(next is TimeSpan, "Next should be TimeSpan");
			Assert.IsTrue((TimeSpan)next > (TimeSpan)current, "next should be greater than current (could be equal depending on how quickly this occurs)");
		}

		[Test]
		public async Task SeedAsync()
		{
			var type = (TimeSpanType)NHibernateUtil.TimeSpan;
			Assert.IsTrue(await (type.SeedAsync(null)) is TimeSpan, "seed should be TimeSpan");
		}
	}

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class TimeSpanTypeFixture2Async : TypeFixtureBaseAsync
	{
		protected override string TypeName
		{
			get
			{
				return "TimeSpan";
			}
		}

		[Test]
		public async Task SavingAndRetrievingAsync()
		{
			var ticks = new TimeSpan(1982);
			var entity = new TimeSpanClass{TimeSpanValue = ticks};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(entity));
					await (tx.CommitAsync());
				}

			TimeSpanClass entityReturned;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					entityReturned = await (s.CreateQuery("from TimeSpanClass").UniqueResultAsync<TimeSpanClass>());
					Assert.AreEqual(ticks, entityReturned.TimeSpanValue);
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
