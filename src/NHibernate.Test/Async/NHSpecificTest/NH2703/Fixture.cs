#if NET_4_5
using NUnit.Framework;
using System.Threading.Tasks;

namespace NHibernate.Test.NHSpecificTest.NH2703
{
	[TestFixture]
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public partial class FixtureAsync : BugTestCaseAsync
	{
		Parent RootElement = null;
		protected override async Task OnSetUpAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				var parent = new Parent();
				parent.A.Add(new A()
				{PropA = "Child"});
				parent.B.Add(new B()
				{PropB = "Child"});
				parent.C.Add(new C()
				{PropC = "Child"});
				await (session.PersistAsync(parent));
				await (session.FlushAsync());
			}
		}

		protected override async Task OnTearDownAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				await (session.CreateQuery("delete from A").ExecuteUpdateAsync());
				await (session.CreateQuery("delete from B").ExecuteUpdateAsync());
				await (session.CreateQuery("delete from C").ExecuteUpdateAsync());
				await (session.CreateQuery("delete from Parent").ExecuteUpdateAsync());
				await (session.FlushAsync());
			}

			await (base.OnTearDownAsync());
		}

		[Test]
		public async Task CanOuterJoinMultipleTablesWithSimpleWithClauseAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				IQueryOver<Parent, Parent> query = session.QueryOver(() => RootElement);
				A A_Alias = null;
				B B_Alias = null;
				C C_Alias = null;
				query.Left.JoinQueryOver(parent => parent.C, () => C_Alias, c => c.PropC == A_Alias.PropA);
				query.Left.JoinQueryOver(parent => parent.A, () => A_Alias);
				query.Left.JoinQueryOver(parent => parent.B, () => B_Alias, b => b.PropB == C_Alias.PropC);
				// Expected join order: a --> c --> b
				// This query should not throw
				await (query.ListAsync());
			}
		}

		[Test]
		public async Task CanOuterJoinMultipleTablesWithComplexWithClauseAsync()
		{
			using (ISession session = Sfi.OpenSession())
			{
				IQueryOver<Parent, Parent> query = session.QueryOver(() => RootElement);
				A A_Alias = null;
				B B_Alias = null;
				C C_Alias = null;
				query.Left.JoinQueryOver(parent => parent.C, () => C_Alias, c => c.PropC == A_Alias.PropA && c.PropC == B_Alias.PropB);
				query.Left.JoinQueryOver(parent => parent.A, () => A_Alias);
				query.Left.JoinQueryOver(parent => parent.B, () => B_Alias, b => b.PropB == A_Alias.PropA);
				// Expected join order: a --> b --> c
				// This query should not throw
				await (query.ListAsync());
			}
		}
	}
}
#endif
