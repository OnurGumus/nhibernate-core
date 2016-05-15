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
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
