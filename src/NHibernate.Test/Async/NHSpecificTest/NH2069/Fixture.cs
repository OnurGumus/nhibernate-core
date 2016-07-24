#if NET_4_5
using System;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using NHibernate;
using NHibernate.Impl;
using NHibernate.Proxy;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2069
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					var test2 = new Test2();
					test2.Cid = 5;
					test2.Description = "Test 2: CID = 5";
					var test = new Test();
					test.Cid = 1;
					test.Description = "Test: CID = 1";
					test.Category = test2;
					await (s.SaveAsync(test2));
					await (s.SaveAsync(test));
					await (s.Transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (s.BeginTransaction())
				{
					await (s.DeleteAsync("from Test"));
					await (s.DeleteAsync("from Test2"));
					await (s.Transaction.CommitAsync());
				}
		}

		[Test]
		public async Task ProxyRemainsUninitializedWhenReferencingIdPropertyAsync()
		{
			using (ISession session = base.OpenSession())
			{
				ITest b = await (session.CreateQuery("from Test").UniqueResultAsync<Test>());
				Assert.IsNotNull(b);
				INHibernateProxy proxy = b.Category as INHibernateProxy;
				Assert.That(proxy, Is.Not.Null);
				Assert.That(proxy.HibernateLazyInitializer.IsUninitialized, "Proxy should be uninitialized.");
				long cid = b.Category.Cid;
				Assert.That(proxy.HibernateLazyInitializer.IsUninitialized, "Proxy should still be uninitialized.");
			}
		}
	}
}
#endif
