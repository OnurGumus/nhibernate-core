#if NET_4_5
using NUnit.Framework;
using NHibernate.Cfg;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2112
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task TestAsync()
		{
			A a;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					a = new A();
					a.Name = "A";
					B b1 = new B{Name = "B1"};
					await (s.SaveAsync(b1));
					B b2 = new B{Name = "B2"};
					await (s.SaveAsync(b2));
					a.Map.Add(b1, "B1Text");
					a.Map.Add(b2, "B2Text");
					await (s.SaveAsync(a));
					await (s.FlushAsync());
					await (tx.CommitAsync());
				}

			ClearCounts();
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					A aCopy = (A)s.Merge(a);
					await (s.FlushAsync());
					await (tx.CommitAsync());
				}

			AssertUpdateCount(0);
			AssertInsertCount(0);
		}
	}
}
#endif
