#if NET_4_5
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Impl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Futures
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FutureCriteriaFixtureAsync : FutureFixtureAsync
	{
		[Test]
		public async Task DefaultReadOnlyTestAsync()
		{
			//NH-3575
			using (var s = sessions.OpenSession())
			{
				s.DefaultReadOnly = true;
				var persons = await (s.CreateCriteria(typeof (Person)).FutureAsync<Person>());
				Assert.IsTrue(persons.All(p => s.IsReadOnly(p)));
			}
		}

		[Test]
		public async Task CanUseFutureCriteriaAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
				var persons10 = await (s.CreateCriteria(typeof (Person)).SetMaxResults(10).FutureAsync<Person>());
				var persons5 = await (s.CreateCriteria(typeof (Person)).SetMaxResults(5).FutureAsync<int>());
				using (var logSpy = new SqlLogSpy())
				{
					foreach (var person in persons5)
					{
					}

					foreach (var person in persons10)
					{
					}

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}
		}

		[Test]
		public async Task TwoFuturesRunInTwoRoundTripsAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
				using (var logSpy = new SqlLogSpy())
				{
					var persons10 = await (s.CreateCriteria(typeof (Person)).SetMaxResults(10).FutureAsync<Person>());
					foreach (var person in persons10)
					{
					} // fire first future round-trip

					var persons5 = await (s.CreateCriteria(typeof (Person)).SetMaxResults(5).FutureAsync<int>());
					foreach (var person in persons5)
					{
					} // fire second future round-trip

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(2, events.Length);
				}
			}
		}

		[Test]
		public async Task CanCombineSingleFutureValueWithEnumerableFuturesAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
				var persons = await (s.CreateCriteria(typeof (Person)).SetMaxResults(10).FutureAsync<Person>());
				var personCount = await (s.CreateCriteria(typeof (Person)).SetProjection(Projections.RowCount()).FutureValueAsync<int>());
				using (var logSpy = new SqlLogSpy())
				{
					int count = personCount.Value;
					foreach (var person in persons)
					{
					}

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}
		}
	}
}
#endif
