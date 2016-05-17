#if NET_4_5
using System;
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1920
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task Can_Query_Without_Collection_Size_ConditionAsync()
		{
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					sess.SaveOrUpdate(new Customer()
					{IsDeleted = false});
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					sess.EnableFilter("state").SetParameter("deleted", false);
					var result = sess.CreateQuery("from Customer c join c.Orders o where c.id > :cid").SetParameter("cid", 0).List();
					Assert.That(result.Count == 0);
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					await (sess.DeleteAsync("from System.Object"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task Can_Query_With_Collection_Size_ConditionAsync()
		{
			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					sess.SaveOrUpdate(new Customer()
					{IsDeleted = false});
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					sess.EnableFilter("state").SetParameter("deleted", false);
					var result = sess.CreateQuery("from Customer c join c.Orders o where c.id > :cid and c.Orders.size > 0").SetParameter("cid", 0).List();
					Assert.That(result.Count == 0);
					await (tx.CommitAsync());
				}

			using (ISession sess = OpenSession())
				using (ITransaction tx = sess.BeginTransaction())
				{
					await (sess.DeleteAsync("from System.Object"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
