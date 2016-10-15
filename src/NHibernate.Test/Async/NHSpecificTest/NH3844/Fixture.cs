#if NET_4_5
using System.Linq;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Engine;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3844
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return !(dialect is FirebirdDialect) && !(dialect is MsSqlCeDialect);
		}

		protected override bool AppliesTo(ISessionFactoryImplementor factory)
		{
			// SQL Server seems unable to match complex group by and select list arguments when running over ODBC.";
			return !(factory.ConnectionProvider.Driver is OdbcDriver);
		}

		protected override async Task OnSetUpAsync()
		{
			var job1 = new Job{Name = "Not a Job", BillingType = BillingType.None};
			var job2 = new Job{Name = "Contract Job", BillingType = BillingType.Fixed};
			var job3 = new Job{Name = "Pay as You Go Job", BillingType = BillingType.Hourly};
			var project1 = new Project{Name = "ProjectOne", Job = job1};
			var compP1_x = new Component()
			{Name = "P1x", Project = project1};
			var compP1_y = new Component()
			{Name = "P1y", Project = project1};
			var project2 = new Project{Name = "ProjectTwo", Job = job2};
			var compP2_x = new Component()
			{Name = "P2x", Project = project2};
			var compP2_y = new Component()
			{Name = "P2y", Project = project2};
			var project3 = new Project{Name = "ProjectThree", Job = job3};
			var compP3_x = new Component()
			{Name = "P3x", Project = project3};
			var compP3_y = new Component()
			{Name = "P3y", Project = project3};
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(job1));
					await (session.SaveAsync(project1));
					await (session.SaveAsync(compP1_x));
					await (session.SaveAsync(compP1_y));
					await (session.SaveAsync(job2));
					await (session.SaveAsync(project2));
					await (session.SaveAsync(compP2_x));
					await (session.SaveAsync(compP2_y));
					await (session.SaveAsync(job3));
					await (session.SaveAsync(project3));
					await (session.SaveAsync(compP3_x));
					await (session.SaveAsync(compP3_y));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 1, Project = project1, Components = {}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 2, Project = project1, Components = {compP1_x}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 3, Project = project1, Components = {compP1_y}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 4, Project = project1, Components = {compP1_x, compP1_y}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 5, Project = project2, Components = {}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 6, Project = project2, Components = {compP2_x}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 7, Project = project2, Components = {compP2_y}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 8, Project = project2, Components = {compP2_x, compP2_y}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 9, Project = project3, Components = {}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 10, Project = project3, Components = {compP3_x}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 11, Project = project3, Components = {compP3_y}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 12, Project = project3, Components = {compP3_x, compP3_y}}));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from TimeRecord"));
					await (session.DeleteAsync("from Component"));
					await (session.DeleteAsync("from Project"));
					await (session.DeleteAsync("from Job"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task ConditionalGroupKeyFromArrayAccessAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var baseQuery = session.Query<TimeRecord>();
					Assert.That(await (baseQuery.SumAsync(x => x.TimeInHours)), Is.EqualTo(78));
					var query = baseQuery.Select(t => new object[]{t}).GroupBy(j => new object[]{((TimeRecord)j[0]).Project.Job.BillingType == BillingType.None ? 0 : 1}, j => (TimeRecord)j[0]).Select(g => new object[]{g.Key, g.Count(), g.Sum(t => (decimal ? )t.TimeInHours)});
					var results = (await (query.ToListAsync())).OrderBy(x => (int)((object[])x[0])[0]);
					Assert.That(results.Select(x => x[1]), Is.EquivalentTo(new[]{4, 8}));
					Assert.That(results.Select(x => x[2]), Is.EquivalentTo(new[]{10, 68}));
					Assert.That(results.Sum(x => (decimal ? )x[2]), Is.EqualTo(78));
					transaction.Rollback();
				}
		}

		[Test]
		public async Task ConditionalGroupKeyFromSubqueryArrayAccessAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var baseQuery = session.Query<TimeRecord>();
					Assert.That(await (baseQuery.SumAsync(x => x.TimeInHours)), Is.EqualTo(78));
					var query = baseQuery.Select(t => new object[]{t}).SelectMany(t => ((TimeRecord)t[0]).Components.Select(c => (object)c.Id).DefaultIfEmpty().Select(c => new[]{t[0], c})).GroupBy(j => new object[]{((TimeRecord)j[0]).Project.Job.BillingType == BillingType.None ? 0 : 1}, j => (TimeRecord)j[0]).Select(g => new object[]{g.Key, g.Count(), g.Sum(t => (decimal ? )t.TimeInHours)});
					var results = (await (query.ToListAsync())).OrderBy(x => (int)((object[])x[0])[0]);
					Assert.That(results.Select(x => x[1]), Is.EquivalentTo(new[]{5, 10}));
					Assert.That(results.Select(x => x[2]), Is.EquivalentTo(new[]{14, 88}));
					Assert.That(results.Sum(x => (decimal ? )x[2]), Is.EqualTo(102));
					transaction.Rollback();
				}
		}

		[Test]
		public async Task ConditionalInComplexGroupKeyFromSubqueryArrayAccessAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var baseQuery = session.Query<TimeRecord>();
					Assert.That(await (baseQuery.SumAsync(x => x.TimeInHours)), Is.EqualTo(78));
					var query = baseQuery.Select(t => new object[]{t}).SelectMany(t => ((TimeRecord)t[0]).Components.Select(c => (object)c.Id).DefaultIfEmpty().Select(c => new[]{t[0], c})).GroupBy(j => new object[]{((TimeRecord)j[0]).Project.Job.BillingType == BillingType.None ? 0 : 1, ((Component)j[1]).Name}, j => (TimeRecord)j[0]).Select(g => new object[]{g.Key, g.Count(), g.Sum(t => (decimal ? )t.TimeInHours)});
					var results = (await (query.ToListAsync())).OrderBy(x => (int)((object[])x[0])[0]).ThenBy(x => (string)((object[])x[0])[1]);
					Assert.That(results.Select(x => x[1]), Is.EquivalentTo(new[]{1, 2, 2, 2, 2, 2, 2, 2}));
					Assert.That(results.Select(x => x[2]), Is.EquivalentTo(new[]{1, 6, 7, 14, 14, 15, 22, 23}));
					Assert.That(results.Sum(x => (decimal ? )x[2]), Is.EqualTo(102));
					transaction.Rollback();
				}
		}
	}
}
#endif
