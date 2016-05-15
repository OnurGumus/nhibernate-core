#if NET_4_5
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.EntityNameAndCompositeId
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanPersistAndReadAsync()
		{
			object id;
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					id = await (s.SaveAsync("Person", new Dictionary<string, object>{{"OuterId", new Dictionary<string, int>{{"InnerId", 1}}}, {"Data", "hello"}}));
					await (tx.CommitAsync());
				}
			}

			using (ISession s = OpenSession())
			{
				using (s.BeginTransaction())
				{
					var p = (IDictionary)await (s.GetAsync("Person", id));
					Assert.AreEqual("hello", p["Data"]);
				}
			}
		}
	}
}
#endif
