#if NET_4_5
using NHibernate.Cfg;
using NHibernate.Criterion;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1452
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task Delete_single_recordAsync()
		{
			using (var session = OpenSession())
			{
				var product = new Product{ProductId = "XO1111", Id = 3, Name = "Test", Description = "Test"};
				await (session.SaveAsync(product));
				await (session.FlushAsync());
				await (session.DeleteAsync(product));
				await (session.FlushAsync());
				session.Clear();
				//try to query for this product
				product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("ProductId", "XO1111")).UniqueResultAsync<Product>());
				Assert.That(product, Is.Null);
			}
		}

		[Test]
		public async Task Query_recordsAsync()
		{
			using (var sqlLog = new SqlLogSpy())
				using (var session = OpenSession())
				{
					var product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("ProductId", "XO1234")).UniqueResultAsync<Product>());
					Assert.That(product, Is.Not.Null);
					Assert.That(product.Description, Is.EqualTo("Very good"));
					var log = sqlLog.GetWholeLog();
					//needs to be joining on the Id column not the productId
					Assert.That(log.Contains("inner join ProductLocalized this_1_ on this_.Id=this_1_.Id"), Is.True);
				}
		}

		[Test]
		public async Task Update_recordAsync()
		{
			using (var session = OpenSession())
			{
				var product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("ProductId", "XO1234")).UniqueResultAsync<Product>());
				Assert.That(product, Is.Not.Null);
				product.Name = "TestValue";
				product.Description = "TestValue";
				await (session.FlushAsync());
				session.Clear();
				//pull again
				product = await (session.CreateCriteria(typeof (Product)).Add(Restrictions.Eq("ProductId", "XO1234")).UniqueResultAsync<Product>());
				Assert.That(product, Is.Not.Null);
				Assert.That(product.Name, Is.EqualTo("TestValue"));
				Assert.That(product.Description, Is.EqualTo("TestValue"));
			}
		}
	}
}
#endif
