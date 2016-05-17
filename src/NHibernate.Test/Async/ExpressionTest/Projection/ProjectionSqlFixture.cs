#if NET_4_5
using System;
using System.Collections;
using NUnit.Framework;
using NHibernate.Criterion;
using NHibernate.Type;
using System.Threading.Tasks;

namespace NHibernate.Test.ExpressionTest.Projection
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class ProjectionSqlFixture : TestCase
	{
		[Test]
		public async Task QueryTestWithStrongTypeReturnValueAsync()
		{
			using (ISession session = OpenSession())
			{
				ICriteria c = session.CreateCriteria(typeof (ProjectionTestClass));
				NHibernate.Transform.IResultTransformer trans = new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof (ProjectionReport).GetConstructors()[0]);
				c.SetProjection(Projections.ProjectionList().Add(Projections.Avg("Pay")).Add(Projections.Max("Pay")).Add(Projections.Min("Pay")));
				c.SetResultTransformer(trans);
				ProjectionReport report = await (c.UniqueResultAsync<ProjectionReport>());
				Assert.AreEqual(report.AvgPay, 2.5);
				Assert.AreEqual(report.MaxPay, 4);
				Assert.AreEqual(report.MinPay, 1);
			}
		}
	}
}
#endif
