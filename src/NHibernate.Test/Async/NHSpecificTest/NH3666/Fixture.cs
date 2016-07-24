#if NET_4_5
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3666
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var session = this.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					var entity1 = new Entity{Id = 1, Property = "Test1"};
					var entity2 = new Entity{Id = 2, Property = "Test2"};
					var entity3 = new Entity{Id = 3, Property = "Test3"};
					await (session.SaveAsync(entity1));
					await (session.SaveAsync(entity2));
					await (session.SaveAsync(entity3));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = this.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Entity"));
					await (transaction.CommitAsync());
				}
		}

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
