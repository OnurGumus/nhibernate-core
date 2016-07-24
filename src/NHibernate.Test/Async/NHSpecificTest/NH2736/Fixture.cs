#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2736
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect.SupportsVariableLimit;
		}

		protected override async Task OnSetUpAsync()
		{
			using (var session = Sfi.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					{
						var order = new SalesOrder{Number = 1};
						order.Items.Add(new Item{SalesOrder = order, Quantity = 1});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 2});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 3});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 4});
						await (session.PersistAsync(order));
					}

					{
						var order = new SalesOrder{Number = 2};
						order.Items.Add(new Item{SalesOrder = order, Quantity = 1});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 2});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 3});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 4});
						await (session.PersistAsync(order));
					}

					{
						var order = new SalesOrder{Number = 3};
						order.Items.Add(new Item{SalesOrder = order, Quantity = 1});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 2});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 3});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 4});
						await (session.PersistAsync(order));
					}

					{
						var order = new SalesOrder{Number = 4};
						order.Items.Add(new Item{SalesOrder = order, Quantity = 1});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 2});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 3});
						order.Items.Add(new Item{SalesOrder = order, Quantity = 4});
						await (session.PersistAsync(order));
					}

					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (var session = Sfi.OpenSession())
				using (var transaction = session.BeginTransaction())
				{
					await (session.CreateQuery("delete from Item").ExecuteUpdateAsync());
					await (session.CreateQuery("delete from SalesOrder").ExecuteUpdateAsync());
					await (transaction.CommitAsync());
				}
		}

		[Test]
		public async Task TestHqlParametersWithTakeAsync()
		{
			using (var session = Sfi.OpenSession())
				using (session.BeginTransaction())
				{
					var query = session.CreateQuery("select o.Id, i.Id from SalesOrder o left join o.Items i with i.Quantity = :pQuantity take :pTake");
					query.SetParameter("pQuantity", 1);
					query.SetParameter("pTake", 2);
					var result = await (query.ListAsync());
					Assert.That(result.Count, Is.EqualTo(2));
				}
		}
	}
}
#endif
