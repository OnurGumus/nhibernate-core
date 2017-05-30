﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.NH1490
{
	using System.Threading.Tasks;
	using System.Threading;
	[TestFixture]
	public class FixtureAsync : BugTestCase
	{
		protected override System.Collections.IList Mappings
		{
			get
			{
				if (Dialect is PostgreSQLDialect)
					return new[] { "NHSpecificTest.NH1490.MappingsFilterAsBoolean.hbm.xml" };

				return base.Mappings;
			}
		}

		[Test]
		public async Task Can_Translate_Correctly_Without_FilterAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Customer c = new Customer("Somebody");
				c.Category = new Category("User");
				c.IsActive = true;
				c.Category.IsActive = true;
				await (s.SaveAsync(c.Category, CancellationToken.None));
				await (s.SaveAsync(c, CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			{
				IQuery query = s.CreateQuery("from Customer c where c.Category.Name = :catName");
				query.SetParameter("catName", "User");
				IList<Customer> customers = await (query.ListAsync<Customer>(CancellationToken.None));

				Assert.That(customers.Count, Is.EqualTo(1), "Can apply condition on Customer without IFilter");
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				await (s.DeleteAsync("from Customer", CancellationToken.None));
				await (s.DeleteAsync("from Category", CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}
		}

		[Test]
		public async Task Also_Works_With_FilterAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Customer c = new Customer("Somebody");
				c.Category = new Category("User");
				await (s.SaveAsync(c.Category, CancellationToken.None));
				c.IsActive = true;
				c.Category.IsActive = true;
				await (s.SaveAsync(c, CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			{
				s.DisableFilter("onlyActive");
				IFilter fltr = s.EnableFilter("onlyActive");
				
				if (Dialect is PostgreSQLDialect)
					fltr.SetParameter("activeFlag", true);
				else
					fltr.SetParameter("activeFlag", 1);

				// Customer is parametrized
				IQuery query = s.CreateQuery("from Customer c where c.Name = :customerName");
				query.SetParameter("customerName", "Somebody");
				IList<Customer> customers = await (query.ListAsync<Customer>(CancellationToken.None));

				Assert.That(customers.Count, Is.EqualTo(1), "IFilter applied and Customer parametrized on Name also works");
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				await (s.DeleteAsync("from Customer", CancellationToken.None));
				await (s.DeleteAsync("from Category", CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}
		}

		[Test]
		public async Task Incorrect_SQL_Translated_Params_BugAsync()
		{
			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				Customer c = new Customer("Somebody");
				c.Category = new Category("User");
				await (s.SaveAsync(c.Category, CancellationToken.None));
				c.IsActive = true;
				c.Category.IsActive = true;
				await (s.SaveAsync(c, CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}

			using (ISession s = OpenSession())
			{
				s.DisableFilter("onlyActive");
				IFilter fltr = s.EnableFilter("onlyActive");
				if (Dialect is PostgreSQLDialect)
					fltr.SetParameter("activeFlag", true);
				else
					fltr.SetParameter("activeFlag", 1);

				// related entity Customer.Category is parametrized
				IQuery query = s.CreateQuery("from Customer c where c.Category.Name = :catName");
				query.SetParameter("catName", "User");

				IList<Customer> customers = await (query.ListAsync<Customer>(CancellationToken.None));

				Assert.That(customers.Count, Is.EqualTo(1), "IFIlter applied and Customer parametrized on Category.Name DOES NOT work");
			}

			using (ISession s = OpenSession())
			using (ITransaction tx = s.BeginTransaction())
			{
				await (s.DeleteAsync("from Customer", CancellationToken.None));
				await (s.DeleteAsync("from Category", CancellationToken.None));
				await (tx.CommitAsync(CancellationToken.None));
			}
		}
	}
}