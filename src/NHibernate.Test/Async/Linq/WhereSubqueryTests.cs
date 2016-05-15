#if NET_4_5
using System;
using System.Linq;
using System.Linq.Expressions;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class WhereSubqueryTests : LinqTestCase
	{
		[Test(Description = "NH-3002")]
		public async Task HqlOrderLinesWithInnerJoinAndSubQueryAsync()
		{
			var lines = await (session.CreateQuery(@"select c from OrderLine c
join c.Order o
where o.Customer.CustomerId = 'VINET'
	and not exists (from c.Order.Employee.Subordinates x where x.EmployeeId = 100)
").ListAsync<OrderLine>());
			Assert.That(lines.Count, Is.EqualTo(10));
		}

		[Test(Description = "NH-3002")]
		public async Task HqlOrderLinesWithImpliedJoinAndSubQueryAsync()
		{
			var lines = await (session.CreateQuery(@"from OrderLine c
where c.Order.Customer.CustomerId = 'VINET'
	and not exists (from c.Order.Employee.Subordinates x where x.EmployeeId = 100)
").ListAsync<OrderLine>());
			Assert.That(lines.Count, Is.EqualTo(10));
		}
	}
}
#endif
