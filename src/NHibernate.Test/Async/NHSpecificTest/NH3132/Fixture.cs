#if NET_4_5
using System.Collections;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3132
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
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
					product.Name = "First";
					product.Lazy = "Lazy";
					await (session.SaveAsync(product));
					await (tran.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (var session = OpenSession())
				using (var tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Product"));
					await (tran.CommitAsync());
				}
		}

		[Test]
		public async Task Query_returns_correct_nameAsync()
		{
			using (var session = OpenSession())
			{
				Product product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("Name", "First")).UniqueResultAsync<Product>());
				Assert.IsNotNull(product);
				Assert.AreEqual("First", product.Name);
			}
		}

		[Test]
		public async Task Correct_value_gets_savedAsync()
		{
			using (var session = OpenSession())
			{
				var product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("Name", "First")).UniqueResultAsync<Product>());
				Assert.That(product, Is.Not.Null);
				product.Name = "Changed";
				await (session.FlushAsync());
				session.Clear();
				var product1 = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("Name", "Changed")).UniqueResultAsync<Product>());
				Assert.That(product1, Is.Not.Null);
				Assert.That(product1.Name, Is.EqualTo("Changed"));
			}
		}

		[Test]
		public async Task Correct_value_gets_saved_with_lazyAsync()
		{
			using (var session = OpenSession())
			{
				var product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("Name", "First")).UniqueResultAsync<Product>());
				Assert.That(product, Is.Not.Null);
				product.Name = "Changed";
				product.Lazy = "LazyChanged";
				await (session.FlushAsync());
				session.Clear();
				var product1 = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("Name", "Changed")).UniqueResultAsync<Product>());
				Assert.That(product1, Is.Not.Null);
				Assert.That(product1.Name, Is.EqualTo("Changed"));
				Assert.That(product1.Lazy, Is.EqualTo("LazyChanged"));
			}
		}
	}
}
#endif
