#if NET_4_5
using System;
using System.Linq;
using NHibernate.Dialect;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2914
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task Linq_DateTimeDotYear_WorksInOracleAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity>().Where(x => x.CreationTime.Year == DateTime.Today.Year).ToList();
					await (tx.CommitAsync());
					Assert.AreEqual(1, result.Count);
				}
		}

		[Test]
		public async Task Linq_DateTimeDotMonth_WorksInOracleAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity>().Where(x => x.CreationTime.Month == DateTime.Today.Month).ToList();
					await (tx.CommitAsync());
					Assert.AreEqual(1, result.Count);
				}
		}

		[Test]
		public async Task Linq_DateTimeDotDay_WorksInOracleAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity>().Where(x => x.CreationTime.Day == DateTime.Today.Day).ToList();
					await (tx.CommitAsync());
					Assert.AreEqual(1, result.Count);
				}
		}

		[Test]
		public async Task Linq_DateTimeDotHour_WorksInOracleAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity>().Where(x => x.CreationTime.Hour <= 24).ToList();
					await (tx.CommitAsync());
					Assert.AreEqual(1, result.Count);
				}
		}

		[Test]
		public async Task Linq_DateTimeDotMinute_WorksInOracleAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity>().Where(x => x.CreationTime.Minute <= 60).ToList();
					await (tx.CommitAsync());
					Assert.AreEqual(1, result.Count);
				}
		}

		[Test]
		public async Task Linq_DateTimeDotSecond_WorksInOracleAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity>().Where(x => x.CreationTime.Second <= 60).ToList();
					await (tx.CommitAsync());
					Assert.AreEqual(1, result.Count);
				}
		}

		[Test]
		public async Task Linq_DateTimeDotDate_WorksInOracleAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = session.Query<Entity>().Where(x => x.CreationTime.Date == DateTime.Today).ToList();
					await (tx.CommitAsync());
					Assert.AreEqual(1, result.Count);
				}
		}
	}
}
#endif
