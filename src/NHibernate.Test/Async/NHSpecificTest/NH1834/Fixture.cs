#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1834
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			var a = new A{Id = 1};
			var a2 = new A{Id = 2};
			var b = new B{Id = 1};
			using (ISession session = base.OpenSession())
			{
				await (session.SaveAsync(a));
				await (session.SaveAsync(a2));
				await (session.SaveAsync(b));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = base.OpenSession())
			{
				await (session.DeleteAsync("from B"));
				await (session.DeleteAsync("from A"));
				await (session.FlushAsync());
			}
		}

		[Test]
		public async Task OneToManyPropertyWithFormulaNodeShouldWorkLikeFormulaAttribAsync()
		{
			using (ISession session = base.OpenSession())
			{
				session.Clear();
				var b = await (session.GetAsync<B>(1));
				Assert.IsNotNull(b.A2);
				Assert.IsNotNull(b.A);
				Assert.That(b.A.Id == b.A2.Id);
			}
		}
	}
}
#endif
