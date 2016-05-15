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

		[Test]
		public async Task QueryTest1Async()
		{
			using (ISession session = OpenSession())
			{
				ICriteria c = session.CreateCriteria(typeof (ProjectionTestClass));
				c.SetProjection(Projections.ProjectionList().Add(Projections.Avg("Pay")).Add(Projections.Max("Pay")).Add(Projections.Min("Pay")));
				IList result = await (c.ListAsync()); // c.UniqueResult();
				Assert.IsTrue(result.Count == 1, "More than one record was found, while just one was expected");
				Assert.IsTrue(result[0] is object[], "expected object[] as result, but found " + result[0].GetType().Name);
				object[] results = (object[])result[0];
				Assert.AreEqual(results.Length, 3);
				Assert.AreEqual(results[0], 2.5);
				Assert.AreEqual(results[1], 4);
				Assert.AreEqual(results[2], 1);
			}
		}

		[Test]
		public async Task SelectSqlProjectionTestAsync()
		{
			using (ISession session = OpenSession())
			{
				ICriteria c = session.CreateCriteria(typeof (ProjectionTestClass));
				c.SetProjection(Projections.ProjectionList().Add(Projections.SqlProjection("Avg({alias}.Pay) as MyPay", new string[]{"MyPay"}, new IType[]{NHibernateUtil.Double})));
				IList result = await (c.ListAsync()); // c.UniqueResult();
				Assert.IsTrue(result.Count == 1);
				object results = result[0];
				Assert.AreEqual(results, 2.5);
			}
		}
	}
}
#endif
