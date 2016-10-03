#if NET_4_5
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1579
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH1579FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestAsync()
		{
			Cart cart = new Cart("Fred");
			Apple apple = new Apple(cart);
			Orange orange = new Orange(cart);
			cart.Apples.Add(apple);
			cart.Oranges.Add(orange);
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(cart));
					await (tx.CommitAsync());
				}
			}

			using (ISession session = OpenSession())
			{
				IQuery query = session.CreateQuery("FROM Fruit f WHERE f.Container.id = :containerID");
				query.SetGuid("containerID", cart.ID);
				IList<Fruit> fruit = await (query.ListAsync<Fruit>());
				Assert.AreEqual(2, fruit.Count);
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("FROM Entity"));
					await (tx.CommitAsync());
				}
			}
		}
	}
}
#endif
