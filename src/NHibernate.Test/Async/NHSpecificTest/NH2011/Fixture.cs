﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH2011
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		[Test]
		public async Task TestAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.SaveAsync(new Country {CountryCode = "SE"}, CancellationToken.None));
					await (tx.CommitAsync(CancellationToken.None));
				}
			}

			var newOrder = new Order();
			newOrder.GroupComponent = new GroupComponent();
			newOrder.GroupComponent.Countries = new List<Country>();
			newOrder.GroupComponent.Countries.Add(new Country {CountryCode = "SE"});

			Order mergedCopy;
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					mergedCopy = (Order) session.Merge(newOrder);
					await (tx.CommitAsync(CancellationToken.None));
				}
			}

			using (ISession session = OpenSession())
			{
				var order = await (session.GetAsync<Order>(mergedCopy.Id, CancellationToken.None));
				Assert.That(order.GroupComponent.Countries.Count, Is.EqualTo(1));
			}

			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Order", CancellationToken.None));
					await (session.DeleteAsync("from Country", CancellationToken.None));
					await (tx.CommitAsync(CancellationToken.None));
				}
			}
		}
	}
}
