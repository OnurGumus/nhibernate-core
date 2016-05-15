#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1235
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task TestAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					SomeClass obj;
					for (int i = 0; i < 10; i++)
					{
						obj = new SomeClass();
						obj.Name = "someclass " + (i + 1).ToString();
						await (s.SaveAsync(obj));
					}

					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					IQuery qry = s.CreateQuery("from SomeClass").SetMaxResults(5);
					IList<SomeClass> list = await (qry.ListAsync<SomeClass>());
					Assert.AreEqual(5, list.Count, "Should have returned 5 entities");
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
