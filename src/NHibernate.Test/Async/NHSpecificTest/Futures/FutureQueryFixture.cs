#if NET_4_5
using System.Linq;
using NHibernate.Driver;
using NHibernate.Impl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Futures
{
	using System.Collections;

	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FutureQueryFixtureAsync : FutureFixtureAsync
	{
		[Test]
		public async Task DefaultReadOnlyTestAsync()
		{
			//NH-3575
			using (var s = sessions.OpenSession())
			{
				s.DefaultReadOnly = true;
				var persons = await (s.CreateQuery("from Person").FutureAsync<Person>());
				Assert.IsTrue(persons.All(p => s.IsReadOnly(p)));
			}
		}

		[Test]
		public async Task CanUseFutureQueryAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
				var persons10 = await (s.CreateQuery("from Person").SetMaxResults(10).FutureAsync<Person>());
				var persons5 = await (s.CreateQuery("from Person").SetMaxResults(5).FutureAsync<int>());
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
					var persons10 = await (s.CreateQuery("from Person").SetMaxResults(10).FutureAsync<Person>());
					foreach (var person in persons10)
					{
					} // fire first future round-trip

					var persons5 = await (s.CreateQuery("from Person").SetMaxResults(5).FutureAsync<int>());
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
				var persons = await (s.CreateQuery("from Person").SetMaxResults(10).FutureAsync<Person>());
				var personCount = await (s.CreateQuery("select count(*) from Person").FutureValueAsync<long>());
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

		[Test]
		public async Task CanExecuteMultipleQueryWithSameParameterNameAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
				var meContainer = await (s.CreateQuery("from Person p where p.Id = :personId").SetParameter("personId", 1).FutureValueAsync<Person>());
				var possiblefriends = await (s.CreateQuery("from Person p where p.Id != :personId").SetParameter("personId", 2).FutureAsync<Person>());
				using (var logSpy = new SqlLogSpy())
				{
					var me = meContainer.Value;
					foreach (var person in possiblefriends)
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
	}
}
#endif
