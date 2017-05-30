﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH296
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		public override string BugNumber
		{
			get { return "NH296"; }
		}

		[Test]
		public async Task CRUDAsync()
		{
			Stock stock = new Stock();
			stock.ProductPK = new ProductPK();
			stock.ProductPK.Number = 1;
			stock.ProductPK.Type = 1;

			using (ISession s = OpenSession())
			{
				await (s.SaveAsync(stock, CancellationToken.None));
				await (s.FlushAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			{
				stock = (Stock) await (s.GetAsync(typeof(Stock), stock.ProductPK, CancellationToken.None));
				Assert.IsNotNull(stock);
			}

			using (ISession s = OpenSession())
			{
				stock = (Stock) await (s.GetAsync(typeof(Product), stock.ProductPK, CancellationToken.None));
				Assert.IsNotNull(stock);

				stock.Property = 10;
				await (s.FlushAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			{
				await (s.DeleteAsync(stock, CancellationToken.None));
				await (s.FlushAsync(CancellationToken.None));
			}
		}
	}
}