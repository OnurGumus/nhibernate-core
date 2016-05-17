#if NET_4_5
using System;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2632
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : TestCaseMappingByCode
	{
		[Test]
		public async Task GettingCustomerDoesNotThrowAsync()
		{
			using (var scenario = new Scenario(Sfi))
			{
				using (var session = OpenSession())
				{
					Customer customer = null;
					Assert.That(async () => customer = await (session.GetAsync<Customer>(scenario.CustomerId)), Throws.Nothing);
					// An entity defined with lazy=false can't have lazy properties (as reported by the WARNING; see EntityMetamodel class)
					Assert.That(NHibernateUtil.IsInitialized(customer.Address), Is.True);
					Assert.That(customer.Address, Is.EqualTo("Bah?!??"));
				}
			}
		}
	}
}
#endif
