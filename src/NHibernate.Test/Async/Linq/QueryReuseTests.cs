#if NET_4_5
using System.Collections.Generic;
using System.Linq;
using NHibernate.DomainModel.Northwind.Entities;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class QueryReuseTestsAsync : LinqTestCaseAsync
	{
		private IQueryable<User> _query;
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			_query = db.Users;
		}

		private async Task AssertQueryReuseableAsync()
		{
			IList<User> users = await (_query.ToListAsync());
			Assert.AreEqual(3, users.Count);
		}

		[Test]
		public async Task CanReuseAfterFirstAsync()
		{
			var user = await (_query.FirstAsync(u => u.Name == "rahien"));
			Assert.IsNotNull(user);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterFirstOrDefaultAsync()
		{
			var user = await (_query.FirstOrDefaultAsync(u => u.Name == "rahien"));
			Assert.IsNotNull(user);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterSingleAsync()
		{
			var user = await (_query.SingleAsync(u => u.Name == "rahien"));
			Assert.IsNotNull(user);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterSingleOrDefaultAsync()
		{
			User user = await (_query.SingleOrDefaultAsync(u => u.Name == "rahien"));
			Assert.IsNotNull(user);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterAggregateAsync()
		{
			User user = _query.Aggregate((u1, u2) => u1);
			Assert.IsNotNull(user);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterAverageAsync()
		{
			double average = await (_query.AverageAsync(u => u.InvalidLoginAttempts));
			Assert.AreEqual(5.0, average);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterCountAsync()
		{
			int totalCount = await (_query.CountAsync());
			Assert.AreEqual(3, totalCount);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterCountWithPredicateAsync()
		{
			int count = await (_query.CountAsync(u => u.LastLoginDate != null));
			Assert.AreEqual(1, count);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterLongCountAsync()
		{
			long totalCount = await (_query.LongCountAsync());
			Assert.AreEqual(3, totalCount);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterLongCountWithPredicateAsync()
		{
			long totalCount = await (_query.LongCountAsync(u => u.LastLoginDate != null));
			Assert.AreEqual(1, totalCount);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterMaxAsync()
		{
			int max = await (_query.MaxAsync(u => u.InvalidLoginAttempts));
			Assert.AreEqual(6, max);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterMinAsync()
		{
			int min = await (_query.MinAsync(u => u.InvalidLoginAttempts));
			Assert.AreEqual(4, min);
			await (AssertQueryReuseableAsync());
		}

		[Test]
		public async Task CanReuseAfterSumAsync()
		{
			int sum = await (_query.SumAsync(u => u.InvalidLoginAttempts));
			Assert.AreEqual(4 + 5 + 6, sum);
			await (AssertQueryReuseableAsync());
		}
	}
}
#endif
