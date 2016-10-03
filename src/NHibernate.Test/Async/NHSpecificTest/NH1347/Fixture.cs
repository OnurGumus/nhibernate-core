#if NET_4_5
using log4net;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1347
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		private static readonly ILog log = LogManager.GetLogger(typeof (FixtureAsync));
		public override string BugNumber
		{
			get
			{
				return "NH1347";
			}
		}

		[Test]
		public async Task BugAsync()
		{
			if ((Dialect is SQLiteDialect) == false)
				Assert.Ignore("NH-1347 is sqlite specific");
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new A("1")));
					await (s.SaveAsync(new A("2")));
					await (s.SaveAsync(new A("3")));
					await (tx.CommitAsync());
				}

			using (SqlLogSpy spy = new SqlLogSpy())
				using (ISession s = OpenSession())
				{
					A a = await (s.CreateCriteria(typeof (A)).AddOrder(Order.Asc("Name")).SetMaxResults(1).UniqueResultAsync<A>());
					Assert.AreEqual("1", a.Name);
					Assert.IsTrue(spy.Appender.GetEvents()[0].MessageObject.ToString().Contains("limit"));
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from A"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
