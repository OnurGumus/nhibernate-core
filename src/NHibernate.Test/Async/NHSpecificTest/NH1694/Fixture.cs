#if NET_4_5
using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1694
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		private async Task FillDbAsync()
		{
			base.OnSetUp();
			using (ISession session = OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					var newUser = new User();
					var newOrder1 = new Orders{User = newUser, Status = true};
					var newOrder2 = new Orders{User = newUser, Status = true};
					await (session.SaveAsync(newUser));
					await (session.SaveAsync(newOrder1));
					await (session.SaveAsync(newOrder2));
					newUser = new User();
					newOrder1 = new Orders{User = newUser, Status = false};
					await (session.SaveAsync(newUser));
					await (session.SaveAsync(newOrder1));
					await (tran.CommitAsync());
				}
			}
		}

		private async Task CleanupAsync()
		{
			base.OnTearDown();
			using (ISession session = OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Orders"));
					await (session.DeleteAsync("from User"));
					await (tran.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanOrderByExpressionContainingACommaInAPagedQueryAsync()
		{
			await (FillDbAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					ICriteria crit = session.CreateCriteria(typeof (User));
					crit.AddOrder(Order.Desc("OrderStatus"));
					crit.AddOrder(Order.Asc("Id"));
					crit.SetMaxResults(10);
					IList<User> list = await (crit.ListAsync<User>());
					Assert.That(list.Count, Is.EqualTo(2));
					Assert.That(list[0].OrderStatus, Is.EqualTo(2));
					Assert.That(list[1].OrderStatus, Is.EqualTo(1));
					await (tran.CommitAsync());
				}
			}

			await (CleanupAsync());
		}
	}
}
#endif
