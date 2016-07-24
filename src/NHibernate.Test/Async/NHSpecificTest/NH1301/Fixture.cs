#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1301
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1301";
			}
		}

		[Test]
		public async Task TestAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					ClassA a = new ClassA();
					a.BCollection.Add(new ClassB());
					await (s.SaveAsync(a));
					await (s.FlushAsync());
					s.Clear();
					//dont know if proxy should be able to refresh
					//so I eager/join load here just to show it doesn't work anyhow...
					ClassA loaded = (await (s.CreateCriteria(typeof (ClassA)).SetFetchMode("BCollection", FetchMode.Join).ListAsync<ClassA>()))[0];
					Assert.AreEqual(1, a.BCollection.Count);
					loaded.BCollection.RemoveAt(0);
					Assert.AreEqual(0, loaded.BCollection.Count);
					await (s.RefreshAsync(loaded));
					Assert.AreEqual(1, loaded.BCollection.Count);
					await (s.DeleteAsync(loaded));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
