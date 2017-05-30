﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1734
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync:BugTestCase
	{
		protected override void OnSetUp()
		{
			using(var session=this.OpenSession())
			using(var tran=session.BeginTransaction())
			{
				var product = new Product {Amount = 3, Price = 43.2};
				var product2 = new Product { Amount = 3, Price = 43.2 };
				session.Save(product);
				session.Save(product2);
				tran.Commit();
			}
		}
		protected override void OnTearDown()
		{
			using(var session=this.OpenSession())
			using (var tran = session.BeginTransaction())
			{
				session.Delete("from Product");
				tran.Commit();
			}
		}

		[Test]
		public async Task ReturnsApropriateTypeWhenSumUsedWithSomeFormulaAsync()
		{
			using (var session = this.OpenSession())
			using (var tran = session.BeginTransaction())
			{
			    double delta = 0.0000000000001;

				var query=session.CreateQuery("select sum(Amount*Price) from Product");
				var result=await (query.UniqueResultAsync(CancellationToken.None));
				Assert.That(result, Is.InstanceOf(typeof (double)));
                Assert.AreEqual(43.2 * 3 * 2, (double)result, delta);
				query = session.CreateQuery("select sum(Price*Amount) from Product");
				result = await (query.UniqueResultAsync(CancellationToken.None));
				Assert.That(result, Is.InstanceOf(typeof(double)));
                Assert.AreEqual(43.2 * 3 * 2, (double)result, delta);

				query = session.CreateQuery("select sum(Price) from Product");
				result = await (query.UniqueResultAsync(CancellationToken.None));
				Assert.That(result, Is.InstanceOf(typeof(double)));
                Assert.AreEqual(43.2 * 2, (double)result, delta);

				query = session.CreateQuery("select sum(Amount) from Product");
				result = await (query.UniqueResultAsync(CancellationToken.None));
				Assert.That(result, Is.InstanceOf(typeof(Int64)));
				Assert.That(result, Is.EqualTo(6));
			}
		}
	}
}
