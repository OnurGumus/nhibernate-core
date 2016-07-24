#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1502
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Person"));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Person e1 = new Person("Joe", 10, 9);
					Person e2 = new Person("Sally", 100, 8);
					Person e3 = new Person("Tim", 20, 7); //20
					Person e4 = new Person("Fred", 40, 40);
					Person e5 = new Person("Mike", 50, 50);
					await (s.SaveAsync(e1));
					await (s.SaveAsync(e2));
					await (s.SaveAsync(e3));
					await (s.SaveAsync(e4));
					await (s.SaveAsync(e5));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task OrderProjectionTestAsync()
		{
			ISQLFunction arithmaticMultiplication = new VarArgsSQLFunction("(", "*", ")");
			using (ISession session = this.OpenSession())
			{
				ICriteria criteria = session.CreateCriteria(typeof (Person), "c");
				criteria.AddOrder(Order.Asc(Projections.SqlFunction(arithmaticMultiplication, NHibernateUtil.GuessType(typeof (int)), Projections.Property("IQ"), Projections.Constant(-1))));
				IList<Person> results = await (criteria.ListAsync<Person>());
				Assert.AreEqual(5, results.Count);
				Assert.AreEqual("Sally", results[0].Name);
				Assert.AreEqual("Joe", results[4].Name);
			}
		}
	}
}
#endif
