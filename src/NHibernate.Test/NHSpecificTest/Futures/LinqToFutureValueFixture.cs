using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
#if NET_4_5
using System.Threading.Tasks;
#endif

namespace NHibernate.Test.NHSpecificTest.Futures
{
	public partial class LinqToFutureValueFixture : FutureFixture
	{
		[Test]
		public void CanExecuteToFutureValueCount()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var personsCount = session.Query<Person>()
					.Where(x => x.Name == "Test1")
					.ToFutureValue(x => x.Count());

				Assert.AreEqual(1, personsCount.Value);
			}
		}

#if NET_4_5
		[Test]
		public async Task CanExecuteToFutureValueCountAsync()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var personsCount = session.Query<Person>()
					.Where(x => x.Name == "Test1")
					.ToFutureValueAsync(x => x.Count());

				Assert.AreEqual(1, await personsCount.GetValue());
			}
		}
#endif

		[Test]
		public void CanExecuteToFutureValueCountWithPredicate()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var personsCount = session.Query<Person>()
					.ToFutureValue(q => q.Count(x => x.Name == "Test1"));

				Assert.AreEqual(1, personsCount.Value);
			}
		}

#if NET_4_5
		[Test]
		public async Task CanExecuteToFutureValueCountWithPredicateAsync()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var personsCount = session.Query<Person>()
					.ToFutureValueAsync(q => q.Count(x => x.Name == "Test1"));

				Assert.AreEqual(1, await personsCount.GetValue());
			}
		}
#endif

		protected override void OnSetUp()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Save(new Person {Name = "Test1"});
				session.Save(new Person {Name = "Test2"});
				session.Save(new Person {Name = "Test3"});
				session.Save(new Person {Name = "Test4"});
				transaction.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				session.Delete("from System.Object");
				transaction.Commit();
			}
		}
	}
}