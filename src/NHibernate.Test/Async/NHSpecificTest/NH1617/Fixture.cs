#if NET_4_5
using System.Collections.Generic;
using NHibernate.Dialect;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1617
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2005Dialect;
		}

		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					var newUser = new User();
					var newOrder1 = new Order();
					newOrder1.User = newUser;
					newOrder1.Status = true;
					var newOrder2 = new Order();
					newOrder2.User = newUser;
					newOrder2.Status = true;
					await (session.SaveAsync(newUser));
					await (session.SaveAsync(newOrder1));
					await (session.SaveAsync(newOrder2));
					await (tran.CommitAsync());
				}
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Order"));
					await (session.DeleteAsync("from User"));
					await (tran.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanUseDataTypeInFormulaWithCriteriaQueryAsync()
		{
			using (ISession session = OpenSession())
			{
				using (ITransaction tran = session.BeginTransaction())
				{
					string sql = "from User";
					IList<User> list = await (session.CreateQuery(sql).ListAsync<User>());
					Assert.That(list.Count, Is.EqualTo(1));
					Assert.That(list[0].OrderStatus, Is.EqualTo(2));
				}
			}
		}
	}
}
#endif
