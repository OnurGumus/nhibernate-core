#if NET_4_5
using System;
using System.Collections;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1556
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanOrderByAggregateAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					var loadedPatient = await (session.GetAsync<Patient>(patient.Id));
					IList list = session.CreateQuery(@"select p.Id, p.ProductName, max(c.LastFilled), count(c.Id)
from Claim as c
join c.ProductIdentifier.Product as p
where c.Patient = :patient
group by p.Id, p.ProductName
order by max(c.LastFilled) asc, p.ProductName").SetParameter("patient", loadedPatient).SetFirstResult(0).SetMaxResults(2).List();
					Assert.AreEqual(2, list.Count);
					Assert.AreEqual(new DateTime(2000, 4, 1), ((object[])list[0])[2]);
					Assert.AreEqual(new DateTime(2001, 1, 1), ((object[])list[1])[2]);
					Assert.AreEqual(1, ((object[])list[0])[3]);
					Assert.AreEqual(2, ((object[])list[1])[3]);
				}
			}
		}
	}
}
#endif
