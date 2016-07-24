#if NET_4_5
using System;
using System.Linq.Expressions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2733
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		Item Item = null;
		protected override async Task OnSetUpAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				var item = new Item();
				await (session.PersistAsync(item));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				await (session.CreateQuery("delete from Item").ExecuteUpdateAsync());
				await (session.FlushAsync());
			}

			await (base.OnTearDownAsync());
		}

		public static Expression<Func<Item, bool>> GetExpression(DateTime startTime)
		{
			return item => item.Details.StartTime == startTime;
		}

		[Test]
		public async Task CanUseExpressionForWhereAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				IQueryOver<Item, Item> query = session.QueryOver(() => Item);
				var start = DateTime.UtcNow;
				query.Where(GetExpression(start));
				await (query.ListAsync());
			}
		}
	}
}
#endif
