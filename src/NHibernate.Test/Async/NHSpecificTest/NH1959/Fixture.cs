#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1959
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class Fixture : BugTestCase
	{
		[Test]
		public async Task StartWithEmptyDoAddAndRemoveAsync()
		{
			ClassB b = new ClassB();
			ClassA a = new ClassA();
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(a));
					await (s.SaveAsync(b));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					ClassA loadedA = await (s.GetAsync<ClassA>(a.Id));
					ClassB loadedB = await (s.GetAsync<ClassB>(b.Id));
					loadedA.TheBag.Add(loadedB);
					loadedA.TheBag.Remove(loadedB);
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				Assert.AreEqual(0, (await (s.GetAsync<ClassA>(a.Id))).TheBag.Count);
		}

		[Test]
		public async Task StartWithEmptyDoAddAndRemoveAtAsync()
		{
			ClassB b = new ClassB();
			ClassA a = new ClassA();
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.SaveAsync(a));
					await (s.SaveAsync(b));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					ClassA loadedA = await (s.GetAsync<ClassA>(a.Id));
					ClassB loadedB = await (s.GetAsync<ClassB>(b.Id));
					loadedA.TheBag.Add(loadedB);
					loadedA.TheBag.RemoveAt(0);
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				Assert.AreEqual(0, (await (s.GetAsync<ClassA>(a.Id))).TheBag.Count);
		}
	}
}
#endif
