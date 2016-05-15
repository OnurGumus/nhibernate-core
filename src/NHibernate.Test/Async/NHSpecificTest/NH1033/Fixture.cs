#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Collections;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1033
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanUseClassConstraintAsync()
		{
			using (ISession session = OpenSession())
			{
				var crit = session.CreateCriteria(typeof (Animal), "a").Add(Property.ForName("a.class").Eq(typeof (Animal)));
				var results = await (crit.ListAsync<Animal>());
				Assert.AreEqual(1, results.Count);
				Assert.AreEqual(typeof (Animal), await (NHibernateUtil.GetClassAsync(results[0])));
			}
		}
	}
}
#endif
