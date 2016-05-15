#if NET_4_5
using System;
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1028
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task CanLoadCollectionUsingLeftOuterJoinAsync()
		{
			String itemName = "a";
			String shipName = "blah";
			Item item = new Item();
			item.Name = itemName;
			Ship ship = new Ship();
			ship.Name = shipName;
			item.Ships.Add(ship);
			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(ship));
				await (s.SaveAsync(item));
				await (s.FlushAsync());
			}

			using (ISession s = OpenSession())
			{
				ICriteria criteria = s.CreateCriteria(typeof (Item));
				criteria.CreateCriteria("Ships", "s", JoinType.InnerJoin).Add(Expression.IsNotNull("s.Id"));
				criteria.CreateCriteria("Containers", "c", JoinType.LeftOuterJoin).Add(Expression.IsNull("c.Id"));
				IList<Item> results = await (criteria.ListAsync<Item>());
				Assert.AreEqual(1, results.Count);
				Item loadedItem = results[0];
				Assert.AreEqual(itemName, loadedItem.Name);
				Assert.AreEqual(1, loadedItem.Ships.Count);
				foreach (Ship loadedShip in item.Ships)
				{
					Assert.AreEqual(shipName, loadedShip.Name);
				}

				Assert.That(loadedItem.Containers, Is.Empty);
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync(ship));
				await (s.DeleteAsync(item));
				await (s.FlushAsync());
			}
		}
	}
}
#endif
