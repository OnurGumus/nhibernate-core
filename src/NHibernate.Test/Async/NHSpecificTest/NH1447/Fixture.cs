#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1447
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanQueryByConstantProjectionWithTypeAsync()
		{
			using (ISession s = OpenSession())
			{
				ICriteria c = s.CreateCriteria(typeof (Person)).Add(Restrictions.EqProperty("WantsNewsletter", Projections.Constant(false, NHibernateUtil.Boolean)));
				IList<Person> list = await (c.ListAsync<Person>());
				Assert.AreEqual(1, list.Count);
			}
		}
	}
}
#endif
