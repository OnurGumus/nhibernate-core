#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1362
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		[Test]
		public async Task TestAsync()
		{
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					ClassA a = new ClassA();
					ClassB b = new ClassB();
					ClassC c = new ClassC();
					a.B = b;
					a.B.CCollection.Add(c);
					await (s.SaveAsync(a));
					await (s.FlushAsync());
					s.Clear();
					ClassA loaded = await (s.LoadAsync<ClassA>(a.Id));
					//work with first child object
					loaded.B = null;
					await (s.RefreshAsync(loaded));
					Assert.IsNotNull(loaded);
					Assert.AreEqual(1, loaded.B.CCollection.Count);
					//doesn't work with nested object
					loaded.B.CCollection.Clear();
					await (s.RefreshAsync(loaded));
					Assert.AreEqual(1, loaded.B.CCollection.Count);
					await (s.DeleteAsync(loaded));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
