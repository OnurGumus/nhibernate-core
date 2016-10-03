﻿#if NET_4_5
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1488
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task WorkButAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new CustomerNoSmart("Somebody")));
					CustomerNoSmart c = new CustomerNoSmart("Somebody else");
					c.Category = new CustomerCategory("User");
					await (s.SaveAsync(c.Category));
					await (s.SaveAsync(c));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				IList result = await (s.CreateQuery("select c.Name, cat.Name from CustomerNoSmart c left outer join c.Category cat").ListAsync());
				Assert.That(result.Count, Is.EqualTo(2));
			}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from CustomerNoSmart"));
					await (s.DeleteAsync("from Category"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task BugAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(new Customer("Somebody")));
					Customer c = new Customer("Somebody else");
					c.Category = new CustomerCategory("User");
					await (s.SaveAsync(c.Category));
					await (s.SaveAsync(c));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				IList result = await (s.CreateQuery("select c.Name, cat.Name from Customer c left outer join c.Category cat").ListAsync());
				Assert.That(result.Count, Is.EqualTo(2), "should return Customers, on left outer join, even empty Category");
			}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Customer"));
					await (s.DeleteAsync("from Category"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
