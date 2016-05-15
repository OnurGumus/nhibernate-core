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
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
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
