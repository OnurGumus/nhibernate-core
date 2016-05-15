#if NET_4_5
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2044
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class SampleTest : BugTestCase
	{
		[Test]
		public async Task IgnoreCaseShouldWorkWithCharCorrectlyAsync()
		{
			using (ISession session = this.OpenSession())
			{
				ICriteria criteria = session.CreateCriteria(typeof (DomainClass), "domain");
				criteria.Add(NHibernate.Criterion.Expression.Eq("Symbol", 's').IgnoreCase());
				IList<DomainClass> list = await (criteria.ListAsync<DomainClass>());
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
#endif
