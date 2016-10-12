#if NET_4_5
using System.Linq;
using System.Linq.Dynamic;
using NHibernate.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class DynamicQueryTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task CanQueryWithDynamicOrderByAsync()
		{
			//dynamic orderby clause
			var list = await (db.Users.OrderBy("RegisteredAt").ToListAsync());
			Assert.That(list, Is.Ordered.By("RegisteredAt"));
		}

		[Test(Description = "NH-3239")]
		public async Task CanCahceDynamicLinqAsync()
		{
			//dynamic orderby clause
			var users = db.Users.Cacheable().Fetch(x => x.Role).OrderBy("RegisteredAt");
			await (users.ToListAsync());
			using (var log = new SqlLogSpy())
			{
				await (users.ToListAsync());
				Assert.That(log.GetWholeLog(), Is.Null.Or.Empty);
			}
		}
	}
}
#endif
