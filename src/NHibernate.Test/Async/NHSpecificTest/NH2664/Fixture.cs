#if NET_4_5
using System.Collections;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2664
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
				return new[]{"NHSpecificTest.NH2664.Mappings.hbm.xml"};
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
					Product product = new Product();
					product.ProductId = "1";
					product.Properties["Name"] = "First Product";
					product.Properties["Description"] = "First Description";
					await (session.SaveAsync(product));
					product = new Product();
					product.ProductId = "2";
					product.Properties["Name"] = "Second Product";
					product.Properties["Description"] = "Second Description";
					await (session.SaveAsync(product));
					product = new Product();
					product.ProductId = "3";
					product.Properties["Name"] = "val";
					product.Properties["Description"] = "val";
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
					await (tran.CommitAsync());
				}
			}
		}

		[Test]
		public void Query_DynamicComponent()
		{
			using (var session = OpenSession())
			{
				var product = (
					from p in session.Query<Product>()where p.Properties["Name"] == "First Product"
					select p).Single();
				Assert.IsNotNull(product);
				Assert.AreEqual("First Product", product.Properties["Name"]);
			}
		}

		[Test]
		public void Multiple_Query_Does_Not_Cache()
		{
			using (var session = OpenSession())
			{
				// Query by name
				var product1 = (
					from p in session.Query<Product>()where p.Properties["Name"] == "First Product"
					select p).Single();
				Assert.That(product1.ProductId, Is.EqualTo("1"));
				// Query by description (this test is to verify that the dictionary
				// index isn't cached from the query above.
				var product2 = (
					from p in session.Query<Product>()where p.Properties["Description"] == "Second Description"
					select p).Single();
				Assert.That(product2.ProductId, Is.EqualTo("2"));
			}
		}

		[Test]
		public void Different_Key_In_DynamicComponentDictionary_Returns_Different_Keys()
		{
			using (var session = OpenSession())
			{
				Expression<Func<IEnumerable>> key1 = () => (
					from a in session.Query<Product>()where a.Properties["Name"] == "val"
					select a);
				Expression<Func<IEnumerable>> key2 = () => (
					from a in session.Query<Product>()where a.Properties["Description"] == "val"
					select a);
				var nhKey1 = new NhLinqExpression(key1.Body, sessions);
				var nhKey2 = new NhLinqExpression(key2.Body, sessions);
				Assert.AreNotEqual(nhKey1.Key, nhKey2.Key);
			}
		}
	}
}
#endif
