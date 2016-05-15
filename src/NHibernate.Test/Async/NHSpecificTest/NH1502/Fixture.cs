#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Dialect.Function;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1502
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task OrderProjectionTestAsync()
		{
			ISQLFunction arithmaticMultiplication = new VarArgsSQLFunction("(", "*", ")");
			using (ISession session = this.OpenSession())
			{
				ICriteria criteria = session.CreateCriteria(typeof (Person), "c");
				criteria.AddOrder(Order.Asc(Projections.SqlFunction(arithmaticMultiplication, NHibernateUtil.GuessType(typeof (int)), Projections.Property("IQ"), Projections.Constant(-1))));
				IList<Person> results = await (criteria.ListAsync<Person>());
				Assert.AreEqual(5, results.Count);
				Assert.AreEqual("Sally", results[0].Name);
				Assert.AreEqual("Joe", results[4].Name);
			}
		}
	}
}
#endif
