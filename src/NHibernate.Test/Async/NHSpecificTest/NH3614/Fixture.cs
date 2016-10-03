#if NET_4_5
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH3614
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task CanProjectListOfStringsAsync()
		{
			Guid id;
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					var testEntity = new Entity{SomeStrings = new List<string>{"Hello", "World"}};
					await (s.SaveAsync(testEntity));
					await (tx.CommitAsync());
					id = testEntity.Id;
				}

			using (var s = OpenSession())
			{
				var result = s.Query<Entity>().Where(x => x.Id == id).Select(x => x.SomeStrings).ToList();
				Assert.AreEqual(1, result.Count);
				Assert.AreEqual(2, result.Single().Count);
			}

			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from Entity"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
