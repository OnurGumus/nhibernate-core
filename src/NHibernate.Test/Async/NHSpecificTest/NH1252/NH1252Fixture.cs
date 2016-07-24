#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1252
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH1252FixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1252";
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from SomeClass"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task TestAsync()
		{
			SubClass1 sc1 = new SubClass1();
			sc1.Name = "obj1";
			object savedId;
			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					savedId = await (s.SaveAsync(sc1));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					Assert.IsNull(await (s.GetAsync<SubClass2>(savedId)));
					await (tx.CommitAsync());
				}

			using (ISession s = OpenSession())
				using (ITransaction tx = s.BeginTransaction())
				{
					await (s.LoadAsync<SomeClass>(savedId)); // Load a proxy by the parent class
					Assert.IsNull(await (s.GetAsync<SubClass2>(savedId)));
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
