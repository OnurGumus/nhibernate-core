#if NET_4_5
using System.Collections.Generic;
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH1611OneToOneIdentity
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class NH1611OneToOneIdentityFixtureAsync : BugTestCaseAsync
	{
		public override string BugNumber
		{
			get
			{
				return "NH1611OneToOneIdentity";
			}
		}

		protected override async Task OnTearDownAsync()
		{
			await (base.OnTearDownAsync());
			using (ISession session = OpenSession())
			{
				using (ITransaction tx = session.BeginTransaction())
				{
					await (session.DeleteAsync("from Adjunct"));
					await (session.DeleteAsync("from Primary"));
					await (tx.CommitAsync());
				}
			}
		}

		protected override async Task OnSetUpAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					Primary primary = new Primary();
					primary.ID = 5;
					primary.Description = "blarg";
					Adjunct adjunct = new Adjunct();
					adjunct.ID = 5;
					adjunct.AdjunctDescription = "nuts";
					primary.Adjunct = adjunct;
					await (s.SaveAsync(primary));
					await (s.SaveAsync(adjunct));
					await (tx.CommitAsync());
				}
			}
		}

		[Test]
		public async Task CanQueryOneToOneWithCompositeIdAsync()
		{
			using (ISession s = OpenSession())
			{
				using (ITransaction tx = s.BeginTransaction())
				{
					ICriteria criteria = s.CreateCriteria(typeof (Primary));
					IList<Primary> list = await (criteria.ListAsync<Primary>());
					Assert.AreEqual("blarg", list[0].Description);
					Assert.AreEqual("nuts", list[0].Adjunct.AdjunctDescription);
				}
			}
		}
	}
}
#endif
