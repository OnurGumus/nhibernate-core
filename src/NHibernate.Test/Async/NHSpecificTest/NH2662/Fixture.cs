#if NET_4_5
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2662
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task WhenCastAliasInQueryOverThenDoNotThrowAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var customer = new Customer{Order = new PizzaOrder{OrderDate = DateTime.Now, PizzaName = "Margarita"}};
					var customer2 = new Customer{Order = new Order{OrderDate = DateTime.Now.AddDays(1)}};
					await (session.SaveAsync(customer));
					await (session.SaveAsync(customer2));
					await (session.FlushAsync());
					Assert.That(() =>
					{
						var temp = session.Query<Customer>().Select(c => new
						{
						c.Id, c.Order.OrderDate, ((PizzaOrder)c.Order).PizzaName
						}

						).ToArray();
						foreach (var item in temp)
						{
							Trace.WriteLine(item.PizzaName);
						}
					}

					, Throws.Nothing);
					Assert.That(async () =>
					{
						Order orderAlias = null;
						var results = await (session.QueryOver<Customer>().Left.JoinAlias(o => o.Order, () => orderAlias).OrderBy(() => orderAlias.OrderDate).Asc.SelectList(list => list.Select(o => o.Id).Select(() => orderAlias.OrderDate).Select(() => ((PizzaOrder)orderAlias).PizzaName)).ListAsync<object[]>());
						Assert.That(results.Count, Is.EqualTo(2));
						Assert.That(results[0][2], Is.EqualTo("Margarita"));
					}

					, Throws.Nothing);
				}
		}
	}
}
#endif
