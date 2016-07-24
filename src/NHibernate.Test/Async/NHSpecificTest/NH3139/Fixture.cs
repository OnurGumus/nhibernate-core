#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NHibernate.Proxy;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3139
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : TestCaseAsync
	{
		protected override string MappingsAssembly
		{
			get
			{
				return "NHibernate.Test";
			}
		}

		protected override IList Mappings
		{
			get
			{
				return new string[]{"NHSpecificTest.NH3139.Mappings.hbm.xml"};
			}
		}

		/// <summary>
		/// push some data into the database
		/// Really functions as a save test also 
		/// </summary>
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (var session = OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					Brand brand = new Brand()
					{Name = "Brand"};
					await (session.SaveAsync(brand));
					//this product has no inventory row
					Product product = new Product();
					product.Name = "First";
					product.Brand = brand;
					await (session.SaveAsync(product));
					await (tran.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
			{
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Product"));
					await (session.DeleteAsync("from Inventory"));
					await (session.DeleteAsync("from Brand"));
					await (tran.CommitAsync());
				}
			}
		}

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
