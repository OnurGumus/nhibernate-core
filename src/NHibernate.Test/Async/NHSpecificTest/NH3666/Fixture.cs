#if NET_4_5
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3666
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CacheableDoesNotThrowExceptionWithNativeSQLQueryAsync()
		{
			using (var session = this.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var result = await (session.CreateSQLQuery("SELECT * FROM Entity WHERE Property = 'Test2'").AddEntity(typeof (Entity)).SetCacheable(true).ListAsync<Entity>());
					CollectionAssert.IsNotEmpty(result);
					Assert.AreEqual(1, result.Count);
					Assert.AreEqual(2, result[0].Id);
				}
		}

		[Test]
		public async Task CacheableDoesNotThrowExceptionWithNamedQueryAsync()
		{
			using (var session = this.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var result = await (session.GetNamedQuery("QueryName").SetCacheable(true).SetString("prop", "Test2").ListAsync<Entity>());
					CollectionAssert.IsNotEmpty(result);
					Assert.AreEqual(1, result.Count);
					Assert.AreEqual(2, result[0].Id);
				}
		}
	}
}
#endif
