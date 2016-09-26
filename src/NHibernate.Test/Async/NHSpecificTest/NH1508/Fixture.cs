#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.NHSpecificTest.NH1508
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Engine.ISessionFactoryImplementor factory)
		{
			return factory.ConnectionProvider.Driver.SupportsMultipleQueries;
		}

		protected override async Task OnSetUpAsync()
		{
			var john = new Person();
			john.Name = "John";
			var doc1 = new Document();
			doc1.Person = john;
			doc1.Title = "John's Doc";
			var doc2 = new Document();
			doc2.Title = "Spec";
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.SaveAsync(john));
					await (session.SaveAsync(doc1));
					await (session.SaveAsync(doc2));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (session.DeleteAsync("from Document"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task DoesntThrowExceptionWhenHqlQueryIsGivenAsync()
		{
			using (var session = OpenSession())
				using (session.BeginTransaction())
				{
					var sqlQuery = session.CreateQuery("from Document");
					var q = session.CreateMultiQuery().Add(sqlQuery);
					await (q.ListAsync());
				}
		}
	}
}
#endif
