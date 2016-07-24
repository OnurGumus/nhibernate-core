#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using NHibernate.Linq;
using NHibernate.Test.ExceptionsTest;
using NHibernate.Test.MappingByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3800
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			var tagA = new Tag()
			{Name = "A"};
			var tagB = new Tag()
			{Name = "B"};
			var project1 = new Project{Name = "ProjectOne"};
			var compP1_x = new Component()
			{Name = "PONEx", Project = project1};
			var compP1_y = new Component()
			{Name = "PONEy", Project = project1};
			var project2 = new Project{Name = "ProjectTwo"};
			var compP2_x = new Component()
			{Name = "PTWOx", Project = project2};
			var compP2_y = new Component()
			{Name = "PTWOy", Project = project2};
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(tagA));
					await (session.SaveAsync(tagB));
					await (session.SaveAsync(project1));
					await (session.SaveAsync(compP1_x));
					await (session.SaveAsync(compP1_y));
					await (session.SaveAsync(project2));
					await (session.SaveAsync(compP2_x));
					await (session.SaveAsync(compP2_y));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 1, Project = null, Components = {}, Tags = {tagA}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 2, Project = null, Components = {}, Tags = {tagB}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 3, Project = project1, Tags = {tagA, tagB}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 4, Project = project1, Components = {compP1_x}, Tags = {tagB}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 5, Project = project1, Components = {compP1_y}, Tags = {tagA}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 6, Project = project1, Components = {compP1_x, compP1_y}, Tags = {}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 7, Project = project2, Components = {}, Tags = {tagA, tagB}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 8, Project = project2, Components = {compP2_x}, Tags = {tagB}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 9, Project = project2, Components = {compP2_y}, Tags = {tagA}}));
					await (session.SaveAsync(new TimeRecord{TimeInHours = 10, Project = project2, Components = {compP2_x, compP2_y}, Tags = {}}));
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
					await (session.DeleteAsync("from Tag"));
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task ExpectedHqlAsync()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var baseQuery = session.Query<TimeRecord>();
					Assert.That(baseQuery.Sum(x => x.TimeInHours), Is.EqualTo(55));
					var query = session.CreateQuery(@"
                    select c.Id, count(t), sum(cast(t.TimeInHours as big_decimal)) 
                    from TimeRecord t 
                    left join t.Components as c 
                    group by c.Id");
					var results = await (query.ListAsync<object[]>());
					Assert.That(results.Select(x => x[1]), Is.EquivalentTo(new[]{4, 2, 2, 2, 2}));
					Assert.That(results.Select(x => x[2]), Is.EquivalentTo(new[]{13, 10, 11, 18, 19}));
					Assert.That(results.Sum(x => (decimal ? )x[2]), Is.EqualTo(71));
					transaction.Rollback();
				}
		}

		[Test]
		public void PureLinq()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var baseQuery = session.Query<TimeRecord>();
					var query =
						from t in baseQuery
						from c in t.Components.Select(x => (object)x.Id).DefaultIfEmpty()let r = new object[]{c, t}group r by r[0] into g
							select new[]{g.Key, g.Select(x => x[1]).Count(), g.Select(x => x[1]).Sum(x => (decimal ? )((TimeRecord)x).TimeInHours)};
					var results = query.ToList();
					Assert.That(results.Select(x => x[1]), Is.EquivalentTo(new[]{4, 2, 2, 2, 2}));
					Assert.That(results.Select(x => x[2]), Is.EquivalentTo(new[]{13, 10, 11, 18, 19}));
					Assert.That(results.Sum(x => (decimal ? )x[2]), Is.EqualTo(71));
					transaction.Rollback();
				}
		}

		[Test]
		public void MethodGroup()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var baseQuery = session.Query<TimeRecord>();
					var query = baseQuery.SelectMany(t => t.Components.Select(c => c.Id).DefaultIfEmpty().Select(c => new object[]{c, t})).GroupBy(g => g[0], g => (TimeRecord)g[1]).Select(g => new[]{g.Key, g.Count(), g.Sum(x => (decimal ? )x.TimeInHours)});
					var results = query.ToList();
					Assert.That(results.Select(x => x[1]), Is.EquivalentTo(new[]{4, 2, 2, 2, 2}));
					Assert.That(results.Select(x => x[2]), Is.EquivalentTo(new[]{13, 10, 11, 18, 19}));
					Assert.That(results.Sum(x => (decimal ? )x[2]), Is.EqualTo(71));
					transaction.Rollback();
				}
		}

		[Test]
		public void ComplexExample()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var baseQuery = session.Query<TimeRecord>();
					Assert.That(baseQuery.Sum(x => x.TimeInHours), Is.EqualTo(55));
					var query = baseQuery.Select(t => new object[]{t}).SelectMany(t => ((TimeRecord)t[0]).Components.Select(c => (object)c.Id).DefaultIfEmpty().Select(c => new[]{t[0], c})).SelectMany(t => ((TimeRecord)t[0]).Tags.Select(x => (object)x.Id).DefaultIfEmpty().Select(x => new[]{t[0], t[1], x})).GroupBy(j => new[]{((TimeRecord)j[0]).Project.Id, j[1], j[2]}, j => (TimeRecord)j[0]).Select(g => new object[]{g.Key, g.Count(), g.Sum(t => (decimal ? )t.TimeInHours)});
					var results = query.ToList();
					Assert.That(results.Select(x => x[1]), Is.EquivalentTo(new[]{1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}));
					Assert.That(results.Select(x => x[2]), Is.EquivalentTo(new[]{1, 2, 3, 3, 4, 5, 6, 6, 7, 7, 8, 9, 10, 10}));
					Assert.That(results.Sum(x => (decimal ? )x[2]), Is.EqualTo(81));
					transaction.Rollback();
				}
		}

		[Test]
		public void OuterJoinGroupingWithSubQueryInProjection()
		{
			using (var session = OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var baseQuery = session.Query<TimeRecord>();
					var query = baseQuery.SelectMany(t => t.Components.Select(c => c.Name).DefaultIfEmpty().Select(c => new object[]{c, t})).GroupBy(g => g[0], g => (TimeRecord)g[1]).Select(g => new[]{g.Key, g.Count(), session.Query<Component>().Count(c => c.Name == (string)g.Key)});
					var results = query.ToList();
					Assert.That(results.Select(x => x[1]), Is.EquivalentTo(new[]{4, 2, 2, 2, 2}));
					Assert.That(results.Select(x => x[2]), Is.EquivalentTo(new[]{0, 1, 1, 1, 1}));
					transaction.Rollback();
				}
		}
	}
}
#endif
