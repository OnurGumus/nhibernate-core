#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3139
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCase
	{
		[Test]
		public async Task Inventory_is_nullAsync()
		{
			using (var session = OpenSession())
			{
				Product product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("Name", "First")).UniqueResultAsync<Product>());
				Assert.IsNotNull(product);
				Assert.IsNull(product.Inventory);
				//Second check will fail because we now have a proxy
				Assert.IsTrue(product.Inventory == null);
			}
		}

		[Test]
		public async Task Other_entities_are_still_proxiesAsync()
		{
			using (var session = OpenSession())
			{
				Product product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("Name", "First")).UniqueResultAsync<Product>());
				Assert.IsNotNull(product);
				Assert.That(product.Brand is INHibernateProxy);
			}
		}
	}
}
#endif
