using NHibernate.Driver;
using NHibernate.Linq;
using NUnit.Framework;
using System.Linq;
#if NET_4_5
using System.Threading.Tasks;
#endif

namespace NHibernate.Test.NHSpecificTest.Futures
{
	[TestFixture]
	public partial class LinqFutureFixture : FutureFixture
	{
		[Test]
		public void DefaultReadOnlyTest()
		{
			//NH-3575
			using (var s = sessions.OpenSession())
			{
				s.DefaultReadOnly = true;

				var persons = s.Query<Person>().ToFuture();

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

				var persons = s.Query<Person>().ToFutureAsync();

				Assert.IsTrue(await persons.All(p => s.IsReadOnly(p)));
			}
		}
#endif

		[Test]
		public void CoalesceShouldWorkForFutures()
		{
			int personId;
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var p1 = new Person { Name = "inserted name" };
				var p2 = new Person { Name = null };

				s.Save(p1);
				s.Save(p2);
				personId = p2.Id;
				tx.Commit();
			}

			using (ISession s = OpenSession())
			using (s.BeginTransaction())
			{
				var person = s.Query<Person>().Where(p => (p.Name ?? "e") == "e").ToFutureValue();
				Assert.AreEqual(personId, person.Value.Id);
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}

#if NET_4_5
		[Test]
		public async Task CoalesceShouldWorkForFuturesAsync()
		{
			int personId;
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				var p1 = new Person { Name = "inserted name" };
				var p2 = new Person { Name = null };

				s.Save(p1);
				s.Save(p2);
				personId = p2.Id;
				tx.Commit();
			}

			using (ISession s = OpenSession())
			using (s.BeginTransaction())
			{
				var person = s.Query<Person>().Where(p => (p.Name ?? "e") == "e").ToFutureValueAsync();
				Assert.AreEqual(personId, (await person.GetValue()).Id);
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}
#endif

		[Test]
		public void CanUseToFutureWithContains()
		{
			using (var s = sessions.OpenSession())
			{
				var ids = new[] { 1, 2, 3 };
				var persons10 = s.Query<Person>()
					.Where(p => ids.Contains(p.Id))
					.FetchMany(p => p.Children)
					.Skip(5)
					.Take(10)
					.ToFuture().ToList();

				Assert.IsNotNull(persons10);
			}
		}

#if NET_4_5
		[Test]
		public async Task CanUseToFutureWithContainsAsync()
		{
			using (var s = sessions.OpenSession())
			{
				var ids = new[] { 1, 2, 3 };
				var persons10 = await s.Query<Person>()
					.Where(p => ids.Contains(p.Id))
					.FetchMany(p => p.Children)
					.Skip(5)
					.Take(10)
					.ToFutureAsync().ToList();

				Assert.IsNotNull(persons10);
			}
		}
#endif

		[Test]
		public void CanUseToFutureWithContains2()
		{
			using (var s = sessions.OpenSession())
			{
				var ids = new[] { 1, 2, 3 };
				var persons10 = s.Query<Person>()
					.Where(p => ids.Contains(p.Id))
					.ToFuture()
					.ToList();

				Assert.IsNotNull(persons10);
			}
		}

#if NET_4_5
		[Test]
		public async Task CanUseToFutureWithContains2Async()
		{
			using (var s = sessions.OpenSession())
			{
				var ids = new[] { 1, 2, 3 };
				var persons10 = await s.Query<Person>()
					.Where(p => ids.Contains(p.Id))
					.ToFutureAsync()
					.ToList();

				Assert.IsNotNull(persons10);
			}
		}
#endif

		[Test]
		public void CanUseSkipAndFetchManyWithToFuture()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var p1 = new Person { Name = "Parent" };
				var p2 = new Person { Parent = p1, Name = "Child" };
				p1.Children.Add(p2);
				s.Save(p1);
				s.Save(p2);
				tx.Commit();

				s.Clear(); // we don't want caching
			}

			using (var s = sessions.OpenSession())
			{
				var persons10 = s.Query<Person>()
					.FetchMany(p => p.Children)
					.Skip(5)
					.Take(10)
					.ToFuture();

				var persons5 = s.Query<Person>()
					.ToFuture();

				using (var logSpy = new SqlLogSpy())
				{
					foreach (var person in persons5) { }

					foreach (var person in persons10) { }

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}

#if NET_4_5
		[Test]
		public async Task CanUseSkipAndFetchManyWithToFutureAsync()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var p1 = new Person { Name = "Parent" };
				var p2 = new Person { Parent = p1, Name = "Child" };
				p1.Children.Add(p2);
				s.Save(p1);
				s.Save(p2);
				tx.Commit();

				s.Clear(); // we don't want caching
			}

			using (var s = sessions.OpenSession())
			{
				var persons10 = s.Query<Person>()
					.FetchMany(p => p.Children)
					.Skip(5)
					.Take(10)
					.ToFutureAsync();

				var persons5 = s.Query<Person>()
					.ToFutureAsync();

				using (var logSpy = new SqlLogSpy())
				{
					foreach (var person in await persons5.ToList()) { }

					foreach (var person in await persons10.ToList()) { }

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}
#endif

		[Test]
		public void CanUseFutureQuery()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			{
				var persons10 = s.Query<Person>()
					.Take(10)
					.ToFuture();
				var persons5 = s.Query<Person>()
					.Take(5)
					.ToFuture();

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
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			{
				var persons10 = s.Query<Person>()
					.Take(10)
					.ToFutureAsync();
				var persons5 = s.Query<Person>()
					.Take(5)
					.ToFutureAsync();

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
		public void CanUseFutureQueryWithAnonymousType()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			{
				var persons = s.Query<Person>()
					.Select(p => new { Id = p.Id, Name = p.Name })
					.ToFuture();
				var persons5 = s.Query<Person>()
					.Select(p => new { Id = p.Id, Name = p.Name })
					.Take(5)
					.ToFuture();

				using (var logSpy = new SqlLogSpy())
				{
					persons5.ToList(); // initialize the enumerable
					persons.ToList();

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}
		}

#if NET_4_5
		[Test]
		public async Task CanUseFutureQueryWithAnonymousTypeAsync()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			{
				var persons = s.Query<Person>()
					.Select(p => new { Id = p.Id, Name = p.Name })
					.ToFutureAsync();
				var persons5 = s.Query<Person>()
					.Select(p => new { Id = p.Id, Name = p.Name })
					.Take(5)
					.ToFutureAsync();

				using (var logSpy = new SqlLogSpy())
				{
					await persons5.ToList(); // initialize the enumerable
					await persons.ToList();

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}
		}
#endif

		[Test]
		public void CanUseFutureFetchQuery()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
			
			using (var s = sessions.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var p1 = new Person { Name = "Parent" };
				var p2 = new Person { Parent = p1, Name = "Child" };
				p1.Children.Add(p2);
				s.Save(p1);
				s.Save(p2);
				tx.Commit();

				s.Clear(); // we don't want caching
			}

			using (var s = sessions.OpenSession())
			{
				var persons = s.Query<Person>()
					.FetchMany(p => p.Children)
					.ToFuture();
				var persons10 = s.Query<Person>()
					.FetchMany(p => p.Children)
					.Take(10)
					.ToFuture();

				using (var logSpy = new SqlLogSpy())
				{

					Assert.That(persons.Any(x => x.Children.Any()), "No children found");
					Assert.That(persons10.Any(x => x.Children.Any()), "No children found");

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}

#if NET_4_5
		[Test]
		public async Task CanUseFutureFetchQueryAsync()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var p1 = new Person { Name = "Parent" };
				var p2 = new Person { Parent = p1, Name = "Child" };
				p1.Children.Add(p2);
				s.Save(p1);
				s.Save(p2);
				tx.Commit();

				s.Clear(); // we don't want caching
			}

			using (var s = sessions.OpenSession())
			{
				var persons = s.Query<Person>()
					.FetchMany(p => p.Children)
					.ToFutureAsync();
				var persons10 = s.Query<Person>()
					.FetchMany(p => p.Children)
					.Take(10)
					.ToFutureAsync();

				using (var logSpy = new SqlLogSpy())
				{

					Assert.That(await persons.Any(x => x.Children.Any()), "No children found");
					Assert.That(await persons10.Any(x => x.Children.Any()), "No children found");

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(1, events.Length);
				}
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}
#endif

		[Test]
		public async Task TwoFuturesRunInTwoRoundTripsAsync()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			{
				using (var logSpy = new SqlLogSpy())
				{
					var persons10 = s.Query<Person>()
						.Take(10)
						.ToFutureAsync();

					foreach (var person in await persons10.ToList()) { } // fire first future round-trip

					var persons5 = s.Query<Person>()
						.Take(5)
						.ToFutureAsync();

					foreach (var person in await persons5.ToList()) { } // fire second future round-trip

					var events = logSpy.Appender.GetEvents();
					Assert.AreEqual(2, events.Length);
				}
			}
		}

#if NET_4_5

#endif

		[Test]
		public void CanCombineSingleFutureValueWithEnumerableFutures()
		{
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();
			
			using (var s = sessions.OpenSession())
			{
				var persons = s.Query<Person>()
					.Take(10)
					.ToFuture();

				var personCount = s.Query<Person>()
					.Select(x => x.Id)
					.ToFutureValue();

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
			IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

			using (var s = sessions.OpenSession())
			{
				var persons = s.Query<Person>()
					.Take(10)
					.ToFutureAsync();

				var personCount = s.Query<Person>()
					.Select(x => x.Id)
					.ToFutureValueAsync();

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

		[Test(Description = "NH-2385")]
		public void CanCombineSingleFutureValueWithFetchMany()
		{
			int personId;
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var p1 = new Person { Name = "inserted name" };
				var p2 = new Person { Name = null };

				s.Save(p1);
				s.Save(p2);
				personId = p2.Id;
				tx.Commit();
			}

			using (var s = sessions.OpenSession())
			{
				var meContainer = s.Query<Person>()
								   .Where(x => x.Id == personId)
								   .FetchMany(x => x.Children)
								   .ToFutureValue();

				Assert.AreEqual(personId, meContainer.Value.Id);
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}

#if NET_4_5
		[Test(Description = "NH-2385")]
		public async Task CanCombineSingleFutureValueWithFetchManyAsync()
		{
			int personId;
			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				var p1 = new Person { Name = "inserted name" };
				var p2 = new Person { Name = null };

				s.Save(p1);
				s.Save(p2);
				personId = p2.Id;
				tx.Commit();
			}

			using (var s = sessions.OpenSession())
			{
				var meContainer = s.Query<Person>()
								   .Where(x => x.Id == personId)
								   .FetchMany(x => x.Children)
								   .ToFutureValueAsync();

				Assert.AreEqual(personId, (await meContainer.GetValue()).Id);
			}

			using (var s = OpenSession())
			using (var tx = s.BeginTransaction())
			{
				s.Delete("from Person");
				tx.Commit();
			}
		}
#endif

		[Test]
		public void CanExecuteMultipleQueriesOnSameExpression()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				var meContainer = s.Query<Person>()
					.Where(x => x.Id == 1)
					.ToFutureValue();

				var possiblefriends = s.Query<Person>()
					.Where(x => x.Id != 2)
					.ToFuture();

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

#if NET_4_5
		[Test]
		public async Task CanExecuteMultipleQueriesOnSameExpressionAsync()
		{
			using (var s = sessions.OpenSession())
			{
				IgnoreThisTestIfMultipleQueriesArentSupportedByDriver();

				var meContainer = s.Query<Person>()
					.Where(x => x.Id == 1)
					.ToFutureValueAsync();

				var possiblefriends = s.Query<Person>()
					.Where(x => x.Id != 2)
					.ToFutureAsync();

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