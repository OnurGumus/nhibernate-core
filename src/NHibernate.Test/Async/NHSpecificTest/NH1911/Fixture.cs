#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using NHibernate.Criterion;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1911
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
