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
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is Oracle8iDialect;
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var entity = new Entity{CreationTime = DateTime.Now};
					await (session.SaveAsync(entity));
					await (tx.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Entity"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task Linq_DateTimeDotYear_WorksInOracleAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction tx = session.BeginTransaction())
				{
					var result = await (session.Query<Entity>().Where(x => x.CreationTime.Year == DateTime.Today.Year).ToListAsync());
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
					var result = await (session.Query<Entity>().Where(x => x.CreationTime.Month == DateTime.Today.Month).ToListAsync());
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
					var result = await (session.Query<Entity>().Where(x => x.CreationTime.Day == DateTime.Today.Day).ToListAsync());
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
					var result = await (session.Query<Entity>().Where(x => x.CreationTime.Hour <= 24).ToListAsync());
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
					var result = await (session.Query<Entity>().Where(x => x.CreationTime.Minute <= 60).ToListAsync());
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
					var result = await (session.Query<Entity>().Where(x => x.CreationTime.Second <= 60).ToListAsync());
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
					var result = await (session.Query<Entity>().Where(x => x.CreationTime.Date == DateTime.Today).ToListAsync());
					await (tx.CommitAsync());
					Assert.AreEqual(1, result.Count);
				}
		}
	}
}
#endif
