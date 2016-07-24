#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2366
{
	[Ignore("Not fixed yet.")]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		protected override async Task OnSetUpAsync()
		{
			await (base.OnSetUpAsync());
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					// Note: non-unique values for Value property
					await (session.SaveAsync(new Two()
					{Id = 1, Value = "a"}));
					await (session.SaveAsync(new Two()
					{Id = 2, Value = "b"}));
					await (session.SaveAsync(new Two()
					{Id = 3, Value = "a"}));
					await (transaction.CommitAsync());
				}

			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.SaveAsync(new One()
					{Id = 1, Value = "a"}));
					await (session.SaveAsync(new One()
					{Id = 2, Value = "a"}));
					await (transaction.CommitAsync());
				}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = OpenSession())
				using (ITransaction transaction = session.BeginTransaction())
				{
					await (session.DeleteAsync("from One"));
					await (session.DeleteAsync("from Two"));
					await (transaction.CommitAsync());
				}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task TestAsync()
		{
			using (ISession session = OpenSession())
			{
				Assert.That(async () => await (session.CreateQuery("from One").ListAsync()), Throws.Nothing);
			}
		}
	}
}
#endif
