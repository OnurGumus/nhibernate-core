#if NET_4_5
using System.Collections.Generic;
using System.Text;
using NHibernate.Dialect;
using NHibernate.Mapping;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2530
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task WhenTryToGetHighThenExceptionShouldContainWhereClauseAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var customer = new Customer{Name = "Mengano"};
					Assert.That(async () => await (session.PersistAsync(customer)), Throws.Exception.Message.ContainsSubstring("Entity = 'Customer'"));
				}
		}
	}
}
#endif
