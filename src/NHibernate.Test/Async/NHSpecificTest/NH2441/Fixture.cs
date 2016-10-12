#if NET_4_5
using System.Linq;
using NHibernate.Dialect;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2441
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(NHibernate.Dialect.Dialect dialect)
		{
			return ((dialect is Dialect.SQLiteDialect) || (dialect is Dialect.MsSql2008Dialect));
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Person e1 = new Person("Tuna Toksoz", "Born in Istanbul :Turkey");
					Person e2 = new Person("Tuna Toksoz", "Born in Istanbul :Turkiye");
					await (s.SaveAsync(e1));
					await (s.SaveAsync(e2));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task LinqQueryBooleanSQLiteAsync()
		{
			using (ISession session = OpenSession())
			{
				var query1 = session.Query<Person>().Where(p => true);
				var query2 = session.Query<Person>().Where(p => p.Id != null);
				var query3 = session.Query<Person>();
				Assert.That(await (query1.CountAsync()), Is.EqualTo(await (query2.CountAsync())));
				Assert.That(await (query3.CountAsync()), Is.EqualTo(await (query1.CountAsync())));
			}
		}
	}
}
#endif
