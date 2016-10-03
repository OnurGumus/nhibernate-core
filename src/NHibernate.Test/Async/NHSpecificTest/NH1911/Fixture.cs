#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1911
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();
				await (s.SaveAsync(new LogEvent()
				{Name = "name parameter", Level = "Fatal"}));
				await (s.SaveAsync(new LogEvent()
				{Name = "name parameter", Level = "NonFatal"}));
				await (s.SaveAsync(new LogEvent()
				{Name = "name parameter", Level = "Fatal"}));
				await (t.CommitAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			await (base.OnSetUpAsync());
			using (ISession s = OpenSession())
			{
				ITransaction t = s.BeginTransaction();
				await (s.CreateQuery("delete from System.Object").ExecuteUpdateAsync());
				await (t.CommitAsync());
			}
		}

		[Test]
		public async Task ConditionalAggregateProjectionAsync()
		{
			IProjection isError = Projections.Conditional(Expression.Eq("Level", "Fatal"), Projections.Constant(1), Projections.Constant(0));
			using (ISession s = OpenSession())
			{
				IList<object[]> actual = await (s.CreateCriteria<LogEvent>().Add(Expression.Eq("Name", "name parameter")).SetProjection(Projections.ProjectionList().Add(Projections.RowCount()).Add(Projections.Sum(isError))).ListAsync<object[]>());
				Assert.That(actual.Count, Is.EqualTo(1));
				Assert.That(actual[0][0], Is.EqualTo(3));
				Assert.That(actual[0][1], Is.EqualTo(2));
			}
		}
	}
}
#endif
