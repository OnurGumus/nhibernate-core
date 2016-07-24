#if NET_4_5
using System.Linq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.Linq
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class PropertyMethodMappingTestsAsync : LinqTestCaseAsync
	{
		[Test]
		public void CanExecuteCountInSelectClause()
		{
			var results = db.Timesheets.Select(t => t.Entries.Count).ToList();
			Assert.AreEqual(3, results.Count);
			Assert.AreEqual(0, results[0]);
			Assert.AreEqual(2, results[1]);
			Assert.AreEqual(4, results[2]);
		}

		[Test]
		public void CanExecuteCountInWhereClause()
		{
			var results = db.Timesheets.Where(t => t.Entries.Count >= 2).ToList();
			Assert.AreEqual(2, results.Count);
		}
	}
}
#endif
