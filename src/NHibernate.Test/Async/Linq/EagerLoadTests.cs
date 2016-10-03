#if NET_4_5
using System.Linq;
using NHibernate.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;
using Exception = System.Exception;
using NHibernate.Util;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class EagerLoadTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task CanSelectAndFetchHqlAsync()
		{
			//NH-3075
			var result = await (this.session.CreateQuery("select c from Order o left join o.Customer c left join fetch c.Orders").ListAsync<Customer>());
			session.Close();
			Assert.IsNotEmpty(result);
			Assert.IsTrue(NHibernateUtil.IsInitialized(result[0].Orders));
		}
	}
}
#endif
