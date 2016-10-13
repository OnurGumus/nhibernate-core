using System.Linq;
using NHibernate.Criterion;
using NHibernate.Impl;
using NUnit.Framework;
#if NET_4_5
using System.Threading.Tasks;
#endif

namespace NHibernate.Test.NHSpecificTest.Futures
{
    [TestFixture]
    public partial class FutureCriteriaFixture : FutureFixture
    {
		[Test]
		public void DefaultReadOnlyTest()
		{
			//NH-3575
			using (var s = sessions.OpenSession())
			{
				s.DefaultReadOnly = true;

				var persons = s.CreateCriteria(typeof(Person)).Future<Person>();

				Assert.IsTrue(persons.All(p => s.IsReadOnly(p)));
			}
		}

#if NET_4_5
		[Test]
		public async Task DefaultReadOnlyTestAsync()
		{
			//NH-3575
			using (var s = sessions.OpenSession())
			{
				s.DefaultReadOnly = true;

				var persons = s.CreateCriteria(typeof(Person)).FutureAsync<Person>();

				Assert.IsTrue(await persons.All(p => s.IsReadOnly(p)));
			}
		}
#endif

		[Test]
        public void CanUseFutureCriteria()
        {
            using (var s = sessions.OpenSession())
            {
                IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

                var persons10 = s.CreateCriteria(typeof(Person))
                    .SetMaxResults(10)
                    .Future<Person>();
                var persons5 = s.CreateCriteria(typeof(Person))
                    .SetMaxResults(5)
                    .Future<int>();

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

#if NET_4_5
		[Test]
		public async Task CanUseFutureCriteriaAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				var persons10 = s.CreateCriteria(typeof(Person))
					.SetMaxResults(10)
					.FutureAsync<Person>();
				var persons5 = s.CreateCriteria(typeof(Person))
					.SetMaxResults(5)
					.FutureAsync<int>();

				using (var logSpy = new SqlLogSpy())
				{
					foreach (var person in await persons5.ToList())
					{

					}

					foreach (var person in await persons10.ToList())
					{

					}

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}
		}
#endif

		[Test]
        public void TwoFuturesRunInTwoRoundTrips()
        {
            using (var s = sessions.OpenSession())
            {
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

                using (var logSpy = new SqlLogSpy())
                {
                    var persons10 = s.CreateCriteria(typeof(Person))
                        .SetMaxResults(10)
                        .Future<Person>();

                    foreach (var person in persons10) { } // fire first future round-trip

                    var persons5 = s.CreateCriteria(typeof(Person))
                        .SetMaxResults(5)
                        .Future<int>();

                    foreach (var person in persons5) { } // fire second future round-trip

                    var events = logSpy.Appender.GetEvents();
                    Assert.AreEqual(2, events.Length);
                }
            }
        }

#if NET_4_5
		[Test]
		public async Task TwoFuturesRunInTwoRoundTripsAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				using (var logSpy = new SqlLogSpy())
				{
					var persons10 = s.CreateCriteria(typeof(Person))
						.SetMaxResults(10)
						.FutureAsync<Person>();

					foreach (var person in await persons10.ToList()) { } // fire first future round-trip

					var persons5 = s.CreateCriteria(typeof(Person))
						.SetMaxResults(5)
						.FutureAsync<int>();

					foreach (var person in await persons5.ToList()) { } // fire second future round-trip

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(2, events.Length);
				}
			}
		}
#endif

		[Test]
		public void CanCombineSingleFutureValueWithEnumerableFutures()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				var persons = s.CreateCriteria(typeof(Person))
					.SetMaxResults(10)
					.Future<Person>();

				var personCount = s.CreateCriteria(typeof(Person))
					.SetProjection(Projections.RowCount())
					.FutureValue<int>();

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

#if NET_4_5
		[Test]
		public async Task CanCombineSingleFutureValueWithEnumerableFuturesAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				var persons = s.CreateCriteria(typeof(Person))
					.SetMaxResults(10)
					.FutureAsync<Person>();

				var personCount = s.CreateCriteria(typeof(Person))
					.SetProjection(Projections.RowCount())
					.FutureValueAsync<int>();

				using (var logSpy = new SqlLogSpy())
				{
					int count = await personCount.GetValue();

					foreach (var person in await persons.ToList())
					{

					}

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}
		}
#endif
	}
}
