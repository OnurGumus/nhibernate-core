#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1706
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class KeyPropertyRefFixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task PropertyRefUsesOtherColumnAsync()
		{
			const string ExtraId = "extra";
			var a = new A{Name = "First", ExtraIdA = ExtraId};
			var b = new B{Name = "Second", ExtraIdB = ExtraId};
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(a));
					await (s.SaveAsync(b));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
			{
				var newA = await (s.GetAsync<A>(a.Id));
				Assert.AreEqual(1, newA.Items.Count);
			}

			// cleanup
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from B"));
					await (s.DeleteAsync("from A"));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
