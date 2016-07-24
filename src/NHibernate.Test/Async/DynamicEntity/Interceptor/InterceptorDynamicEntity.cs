#if NET_4_5
using System.Collections;
using NHibernate.Cfg;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.DynamicEntity.Interceptor
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class InterceptorDynamicEntityAsync : TestCaseAsync
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
				return new string[]{"DynamicEntity.Interceptor.Customer.hbm.xml"};
			}
		}

		protected override Task ConfigureAsync(Configuration configuration)
		{
			try
			{
				configuration.SetInterceptor(new ProxyInterceptor());
				return TaskHelper.CompletedTask;
			}
			catch (Exception ex)
			{
				return TaskHelper.FromException<object>(ex);
			}
		}

		[Test]
		public async Task ItAsync()
		{
			// Test saving these dyna-proxies
			ISession session = OpenSession();
			session.BeginTransaction();
			Company company = ProxyHelper.NewCompanyProxy();
			company.Name = "acme";
			await (session.SaveAsync(company));
			Customer customer = ProxyHelper.NewCustomerProxy();
			customer.Name = "Steve";
			customer.Company = company;
			await (session.SaveAsync(customer));
			await (session.Transaction.CommitAsync());
			session.Close();
			Assert.IsNotNull(company.Id, "company id not assigned");
			Assert.IsNotNull(customer.Id, "customer id not assigned");
			// Test loading these dyna-proxies, along with flush processing
			session = OpenSession();
			session.BeginTransaction();
			customer = await (session.LoadAsync<Customer>(customer.Id));
			Assert.IsFalse(NHibernateUtil.IsInitialized(customer), "should-be-proxy was initialized");
			customer.Name = "other";
			await (session.FlushAsync());
			Assert.IsFalse(NHibernateUtil.IsInitialized(customer.Company), "should-be-proxy was initialized");
			await (session.RefreshAsync(customer));
			Assert.AreEqual("other", customer.Name, "name not updated");
			Assert.AreEqual("acme", customer.Company.Name, "company association not correct");
			await (session.Transaction.CommitAsync());
			session.Close();
			// Test detached entity re-attachment with these dyna-proxies
			customer.Name = "Steve";
			session = OpenSession();
			session.BeginTransaction();
			await (session.UpdateAsync(customer));
			await (session.FlushAsync());
			await (session.RefreshAsync(customer));
			Assert.AreEqual("Steve", customer.Name, "name not updated");
			await (session.Transaction.CommitAsync());
			session.Close();
			// Test querying
			session = OpenSession();
			session.BeginTransaction();
			int count = (await (session.CreateQuery("from Customer").ListAsync())).Count;
			Assert.AreEqual(1, count, "querying dynamic entity");
			session.Clear();
			count = (await (session.CreateQuery("from Person").ListAsync())).Count;
			Assert.AreEqual(1, count, "querying dynamic entity");
			await (session.Transaction.CommitAsync());
			session.Close();
			// test deleteing
			session = OpenSession();
			session.BeginTransaction();
			await (session.DeleteAsync(company));
			await (session.DeleteAsync(customer));
			await (session.Transaction.CommitAsync());
			session.Close();
		}
	}
}
#endif
