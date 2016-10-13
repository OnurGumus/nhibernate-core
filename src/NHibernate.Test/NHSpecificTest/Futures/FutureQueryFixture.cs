using System.Linq;
using NHibernate.Driver;
using NHibernate.Impl;
using NUnit.Framework;
#if NET_4_5
using System.Threading.Tasks;
#endif

namespace NHibernate.Test.NHSpecificTest.Futures
{
	using System.Collections;

	[TestFixture]
	public partial class FutureQueryFixture : FutureFixture
	{
		[Test]
		public void DefaultReadOnlyTest()
		{
			//NH-3575
			using (var s = sessions.OpenSession())
			{
				s.DefaultReadOnly = true;

				var persons = s.CreateQuery("from Person").Future<Person>();

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

				var persons = s.CreateQuery("from Person").FutureAsync<Person>();

				Assert.IsTrue(await persons.All(p => s.IsReadOnly(p)));
			}
		}
#endif

		[Test]
		public void CanUseFutureQuery()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				var persons10 = s.CreateQuery("from Person")
					.SetMaxResults(10)
					.Future<Person>();
				var persons5 = s.CreateQuery("from Person")
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
		public async Task CanUseFutureQueryAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				var persons10 = s.CreateQuery("from Person")
					.SetMaxResults(10)
					.FutureAsync<Person>();
				var persons5 = s.CreateQuery("from Person")
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
					var persons10 = s.CreateQuery("from Person")
						.SetMaxResults(10)
						.Future<Person>();

					foreach (var person in persons10) { } // fire first future round-trip

					var persons5 = s.CreateQuery("from Person")
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
					var persons10 = s.CreateQuery("from Person")
						.SetMaxResults(10)
						.FutureAsync<Person>();

					foreach (var person in await persons10.ToList()) { } // fire first future round-trip

					var persons5 = s.CreateQuery("from Person")
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

				var persons = s.CreateQuery("from Person")
					.SetMaxResults(10)
					.Future<Person>();

				var personCount = s.CreateQuery("select count(*) from Person")
					.FutureValue<long>();

				using (var logSpy = new SqlLogSpy())
				{
					long count = personCount.Value;

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

				var persons = s.CreateQuery("from Person")
					.SetMaxResults(10)
					.FutureAsync<Person>();

				var personCount = s.CreateQuery("select count(*) from Person")
					.FutureValueAsync<long>();

				using (var logSpy = new SqlLogSpy())
				{
					long count = await personCount.GetValue();

					foreach (var person in await persons.ToList())
					{
					}

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}
		}
#endif

		[Test]
		public void CanExecuteMultipleQueryWithSameParameterName()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
			
				var meContainer = s.CreateQuery("from Person p where p.Id = :personId")
					.SetParameter("personId", 1)
					.FutureValue<Person>();
			
				var possiblefriends = s.CreateQuery("from Person p where p.Id != :personId")
					.SetParameter("personId", 2)
					.Future<Person>();

				using (var logSpy = new SqlLogSpy())
				{
					var me = meContainer.Value;
			
					foreach (var person in possiblefriends)
					{
					}
			
					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
					var wholeLog = logSpy.GetWholeLog();
					string paramPrefix = ((DriverBase) Sfi.ConnectionProvider.Driver).NamedPrefix;
					Assert.That(wholeLog.Contains(paramPrefix + "p0 = 1 [Type: Int32 (0)], " + paramPrefix + "p1 = 2 [Type: Int32 (0)]"), Is.True);
				}
			}
		}

#if NET_4_5
		[Test]
		public async Task CanExecuteMultipleQueryWithSameParameterNameAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				var meContainer = s.CreateQuery("from Person p where p.Id = :personId")
					.SetParameter("personId", 1)
					.FutureValueAsync<Person>();

				var possiblefriends = s.CreateQuery("from Person p where p.Id != :personId")
					.SetParameter("personId", 2)
					.FutureAsync<Person>();

				using (var logSpy = new SqlLogSpy())
				{
					var me = await meContainer.GetValue();

					foreach (var person in await possiblefriends.ToList())
					{
					}

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
					var wholeLog = logSpy.GetWholeLog();
					string paramPrefix = ((DriverBase)Sfi.ConnectionProvider.Driver).NamedPrefix;
					Assert.That(wholeLog.Contains(paramPrefix + "p0 = 1 [Type: Int32 (0)], " + paramPrefix + "p1 = 2 [Type: Int32 (0)]"), Is.True);
				}
			}
		}
#endif
	}
}
