#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2224
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanQueryBasedOnYearWithInOperatorAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction t = s.BeginTransaction())
				{
					var criteria = s.CreateCriteria<Class1>();
					criteria.Add(Restrictions.In(Projections.SqlFunction("year", NHibernateUtil.DateTime, Projections.Property("DateOfChange")), new string[]{"2010", DateTime.Now.Year.ToString()}));
					var result = await (criteria.ListAsync());
					Assert.That(result.Count, Is.EqualTo(1));
				}
		}
	}
}
#endif
