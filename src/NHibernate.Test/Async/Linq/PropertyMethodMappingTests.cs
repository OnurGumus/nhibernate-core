#if NET_4_5
using System.Linq;
using NUnit.Framework;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PropertyMethodMappingTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public async Task CanExecuteCountInSelectClauseAsync()
		{
			var results = await (db.Timesheets.Select(t => t.Entries.Count).ToListAsync());
			Assert.AreEqual(3, results.Count);
			Assert.AreEqual(0, results[0]);
			Assert.AreEqual(2, results[1]);
			Assert.AreEqual(4, results[2]);
		}

		[Test]
		public async Task CanExecuteCountInWhereClauseAsync()
		{
			var results = await (db.Timesheets.Where(t => t.Entries.Count >= 2).ToListAsync());
			Assert.AreEqual(2, results.Count);
		}
	}
}
#endif
