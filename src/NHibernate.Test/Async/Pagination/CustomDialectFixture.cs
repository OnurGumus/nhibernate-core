#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;
using System.Threading.Tasks;

namespace NHibernate.Test.Pagination
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class CustomDialectFixture : TestCase
	{
		[Test]
		public async Task LimitFirstAsync()
		{
			using (ISession s = OpenSession())
			{
				CustomDialect.ForcedSupportsVariableLimit = true;
				CustomDialect.ForcedBindLimitParameterFirst = true;
				var points = await (s.CreateCriteria<DataPoint>().Add(Restrictions.Gt("X", 5.1d)).AddOrder(Order.Asc("X")).SetFirstResult(1).SetMaxResults(2).ListAsync<DataPoint>());
				Assert.That(points.Count, Is.EqualTo(2));
				Assert.That(points[0].X, Is.EqualTo(7d));
				Assert.That(points[1].X, Is.EqualTo(8d));
			}
		}

		[Test]
		public async Task LimitFirstMultiCriteriaAsync()
		{
			using (ISession s = OpenSession())
			{
				CustomDialect.ForcedSupportsVariableLimit = true;
				CustomDialect.ForcedBindLimitParameterFirst = true;
				var criteria = s.CreateMultiCriteria().Add<DataPoint>(s.CreateCriteria<DataPoint>().Add(Restrictions.Gt("X", 5.1d)).AddOrder(Order.Asc("X")).SetFirstResult(1).SetMaxResults(2));
				var points = (IList<DataPoint>)(await (criteria.ListAsync()))[0];
				Assert.That(points.Count, Is.EqualTo(2));
				Assert.That(points[0].X, Is.EqualTo(7d));
				Assert.That(points[1].X, Is.EqualTo(8d));
			}
		}
	}
}
#endif
