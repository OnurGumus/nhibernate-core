#if NET_4_5
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Impl;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Futures
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FutureQueryOverFixture : FutureFixture
	{
		[Test]
		public async Task DefaultReadOnlyTestAsync()
		{
			//NH-3575
			using (var s = sessions.OpenSession())
			{
				s.DefaultReadOnly = true;
				var persons = await (s.QueryOver<Person>().FutureAsync<Person>());
				Assert.IsTrue(persons.All(p => s.IsReadOnly(p)));
			}
		}

		[Test]
		public async Task CanUseFutureCriteriaAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
				var persons10 = await (s.QueryOver<Person>().Take(10).FutureAsync());
				var persons5 = await (s.QueryOver<Person>().Select(p => p.Id).Take(5).FutureAsync<int>());
				using (var logSpy = new SqlLogSpy())
				{
					int actualPersons5Count = 0;
					foreach (var person in persons5)
						actualPersons5Count++;
					int actualPersons10Count = 0;
					foreach (var person in persons10)
						actualPersons10Count++;
					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
					Assert.That(actualPersons5Count, Is.EqualTo(1));
					Assert.That(actualPersons10Count, Is.EqualTo(1));
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
					var persons10 = await (s.QueryOver<Person>().Take(10).FutureAsync());
					foreach (var person in persons10)
					{
					} // fire first future round-trip

					var persons5 = await (s.QueryOver<Person>().Select(p => p.Id).Take(5).FutureAsync<int>());
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
				var persons = await (s.QueryOver<Person>().Take(10).FutureAsync());
				var personIds = await (s.QueryOver<Person>().Select(p => p.Id).FutureValueAsync<int>());
				var singlePerson = await (s.QueryOver<Person>().FutureValueAsync());
				using (var logSpy = new SqlLogSpy())
				{
					Person singlePersonValue = singlePerson.Value;
					int personId = personIds.Value;
					foreach (var person in persons)
					{
					}

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
					Assert.That(singlePersonValue, Is.Not.Null);
					Assert.That(personId, Is.Not.EqualTo(0));
				}
			}
		}
	}
}
#endif
