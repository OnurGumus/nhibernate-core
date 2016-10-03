#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1393
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
		public async Task CanSumProjectionOnSqlFunctionAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.Sum(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList list = await (c.ListAsync());
				Assert.AreEqual(334, list[0]);
			}
		}

		[Test]
		public async Task CanAvgProjectionOnSqlFunctionAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.Avg(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList list = await (c.ListAsync());
				Assert.AreEqual(((double)334) / 5, list[0]);
			}
		}

		[Test]
		public async Task CanMinProjectionOnIdentityProjectionAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.Min(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList list = await (c.ListAsync());
				Assert.AreEqual(19, list[0]);
			}
		}

		[Test]
		public async Task CanMaxProjectionOnIdentityProjectionAsync()
		{
			using (ISession s = OpenSession())
			{
				ISQLFunction arithmaticAddition = new VarArgsSQLFunction("(", "+", ")");
				ICriteria c = s.CreateCriteria(typeof (Person)).SetProjection(Projections.Max(Projections.SqlFunction(arithmaticAddition, NHibernateUtil.GuessType(typeof (double)), Projections.Property("IQ"), Projections.Property("ShoeSize"))));
				IList list = await (c.ListAsync());
				Assert.AreEqual(108, list[0]);
			}
		}
	}
}
#endif
