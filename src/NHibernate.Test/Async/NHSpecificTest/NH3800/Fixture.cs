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
using NHibernate.Util;

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
	}
}
#endif
