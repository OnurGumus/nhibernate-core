#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2341
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnTearDownAsync()
		{
			using (var s = OpenSession())
				using (var tx = s.BeginTransaction())
				{
					await (s.DeleteAsync("from System.Object"));
					await (tx.CommitAsync());
				}
		}

		[Test]
		public async Task WhenSaveInstanceOfConcreteInheritedThenNotThrowsAsync()
		{
			using (var session = OpenSession())
				using (var tx = session.BeginTransaction())
				{
					var entity = new ConcreteB();
					Assert.That(async () => await (session.SaveAsync(entity)), Throws.Nothing);
					await (tx.CommitAsync());
				}
		}
	}
}
#endif
