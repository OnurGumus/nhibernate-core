#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.Futures
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class LinqToFutureValueFixtureAsync : FutureFixtureAsync
	{
		[Test]
		public void CanExecuteToFutureValueCount()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var personsCount = session.Query<Person>().Where(x => x.Name == "Test1").ToFutureValue(x => x.Count());
					Assert.AreEqual(1, personsCount.Value);
				}
		}

		[Test]
		public void CanExecuteToFutureValueCountWithPredicate()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var personsCount = session.Query<Person>().ToFutureValue(q => q.Count(x => x.Name == "Test1"));
					Assert.AreEqual(1, personsCount.Value);
				}
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new Person{Name = "Test1"}));
					await (session.SaveAsync(new Person{Name = "Test2"}));
					await (session.SaveAsync(new Person{Name = "Test3"}));
					await (session.SaveAsync(new Person{Name = "Test4"}));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from System.Object"));
					await (transaction.CommitAsync());
				}
		}
	}
}
#endif
